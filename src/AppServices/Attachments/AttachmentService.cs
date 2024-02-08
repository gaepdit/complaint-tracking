using AutoMapper;
using Cts.AppServices.Attachments.Dto;
using Cts.AppServices.ErrorLogging;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities.Attachments;
using Cts.Domain.Entities.Complaints;
using GaEpd.FileService;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Cts.AppServices.Attachments;

public class AttachmentService(
    IFileService fileService,
    IAttachmentManager attachmentManager,
    IAttachmentRepository attachmentRepository,
    IComplaintRepository complaintRepository,
    IUserService userService,
    IMapper mapper,
    IErrorLogger errorLogger)
    : IAttachmentService
{
    private IAttachmentService.AttachmentServiceConfig Config { get; set; } = null!;

    public async Task<AttachmentViewDto?> FindAttachmentAsync(Guid id, CancellationToken token = default) =>
        mapper.Map<AttachmentViewDto>(await attachmentRepository
            .FindAsync(AttachmentFilters.IdPredicate(id), token).ConfigureAwait(false));

    public async Task<AttachmentViewDto?> FindPublicAttachmentAsync(Guid id, CancellationToken token = default) =>
        mapper.Map<AttachmentViewDto>(await attachmentRepository
            .FindAsync(AttachmentFilters.PublicIdPredicate(id), token).ConfigureAwait(false));

    public async Task<byte[]> GetAttachmentFileAsync(string fileId, bool getThumbnail,
        IAttachmentService.AttachmentServiceConfig config, CancellationToken token = default)
    {
        Config = config;
        await using var response = await fileService.TryGetFileAsync(fileId, ExpandPath(fileId, getThumbnail), token)
            .ConfigureAwait(false);
        if (!response.Success) return [];

        using var ms = new MemoryStream();
        await response.Value.CopyToAsync(ms, token).ConfigureAwait(false);
        return ms.ToArray();
    }

    public async Task DeleteAttachmentFileAsync(string fileId, bool isImage,
        IAttachmentService.AttachmentServiceConfig config)
    {
        // TODO: Delete Attachment entity also.
        Config = config;
        if (string.IsNullOrEmpty(fileId)) return;
        await fileService.DeleteFileAsync(fileId, ExpandPath(fileId)).ConfigureAwait(false);
        if (isImage) await fileService.DeleteFileAsync(fileId, ExpandPath(fileId, true)).ConfigureAwait(false);
    }

    public async Task SaveAttachmentsAsync(AttachmentsCreateDto resource,
        IAttachmentService.AttachmentServiceConfig config, CancellationToken token = default)
    {
        Config = config;
        var complaint = await complaintRepository.GetAsync(resource.ComplaintId, token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        foreach (var formFile in resource.FormFiles)
        {
            if (formFile.Length == 0 || string.IsNullOrWhiteSpace(formFile.FileName)) continue;

            var attachment = attachmentManager.Create(formFile, complaint, currentUser);
            attachment.IsImage = await SaveFileAsync(formFile, attachment.FileId).ConfigureAwait(false);
            await attachmentRepository.InsertAsync(attachment, token: token).ConfigureAwait(false);
        }
    }

    private string ExpandPath(string fileId, bool thumbnail = false) =>
        $"{(thumbnail ? Config.ThumbnailsFolder : Config.AttachmentsFolder)}/{fileId[..2]}";

    // SaveFileAsync returns true if formFile is an image; otherwise false.
    private async Task<bool> SaveFileAsync(IFormFile formFile, string fileId)
    {
        // Try to save using the image service (which handles image rotation and thumbnail generation).
        // If successful, file is an image type.
        if (await TrySaveImageAsync(formFile, fileId).ConfigureAwait(false)) return true;

        // If image service fails, save file directly. File is not an image type.
        await fileService.SaveFileAsync(formFile.OpenReadStream(), fileId, ExpandPath(fileId)).ConfigureAwait(false);
        return false;
    }

    private async Task<bool> TrySaveImageAsync(IFormFile formFile, string fileId)
    {
        if (!FileTypes.FileNameImpliesImage(formFile.FileName.Trim())) return false;

        try
        {
            using var image = await Image.LoadAsync(formFile.OpenReadStream()).ConfigureAwait(false);

            // Save full size image.
            await SaveImageAsFileAsync(image, fileId).ConfigureAwait(false);

            // Save thumbnail.
            image.Mutate(context => context
                .Resize(new ResizeOptions { Size = new Size(Config.ThumbnailSize), Mode = ResizeMode.Pad })
                .BackgroundColor(Color.White));
            await SaveThumbnailAsFileAsync(image, fileId).ConfigureAwait(false);

            return true;
        }
        catch (Exception ex)
        {
            // Log error but take no other action here
            var customData = new Dictionary<string, object>
            {
                { "Action", "Saving Image" },
                { "IFormFile", formFile },
                { "File ID", fileId },
            };
            await errorLogger.LogErrorAsync(ex, customData).ConfigureAwait(false);
            return false;
        }
    }

    private Task SaveThumbnailAsFileAsync(Image image, string fileId) =>
        SaveImageAsFileAsync(image, fileId, asThumbnail: true);

    private async Task SaveImageAsFileAsync(Image image, string fileId, bool asThumbnail = false)
    {
        await using var ms = new MemoryStream();
        await image.SaveAsync(ms, image.Metadata.DecodedImageFormat!).ConfigureAwait(false);
        await fileService.SaveFileAsync(ms, fileId, ExpandPath(fileId, asThumbnail)).ConfigureAwait(false);
    }
}
