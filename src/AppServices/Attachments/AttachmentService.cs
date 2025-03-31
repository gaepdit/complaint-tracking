using AutoMapper;
using Cts.AppServices.Attachments.Dto;
using Cts.AppServices.Complaints.QueryDto;
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
    // ReSharper disable once SuggestBaseTypeForParameterInConstructor
    IMapper mapper,
    IErrorLogger errorLogger)
    : IAttachmentService
{
    private IAttachmentService.AttachmentServiceConfig Config { get; set; } = null!;

    public async Task<AttachmentViewDto?> FindAttachmentAsync(Guid id, CancellationToken token = default) =>
        mapper.Map<AttachmentViewDto>(await attachmentRepository
            .FindAsync(AttachmentFilters.IdPredicate(id), token: token).ConfigureAwait(false));

    public async Task<AttachmentViewDto?> FindPublicAttachmentAsync(Guid id, CancellationToken token = default) =>
        mapper.Map<AttachmentViewDto>(await attachmentRepository
            .FindAsync(AttachmentFilters.PublicIdPredicate(id), token: token).ConfigureAwait(false));

    public async Task<ComplaintViewDto?> FindComplaintForAttachmentAsync(Guid attachmentId,
        CancellationToken token = default)
    {
        var attachment = await attachmentRepository.FindAsync(AttachmentFilters.IdPredicate(attachmentId), token: token)
            .ConfigureAwait(false);
        return attachment == null
            ? null
            : mapper.Map<ComplaintViewDto>(await complaintRepository
                .FindAsync(complaint => complaint.Attachments.Contains(attachment), token: token).ConfigureAwait(false));
    }

    public async Task<byte[]> GetAttachmentFileAsync(string fileId, bool getThumbnail,
        IAttachmentService.AttachmentServiceConfig config, CancellationToken token = default)
    {
        Config = config;
        var response = await fileService.TryGetFileAsync(fileId, ExpandPath(fileId, getThumbnail), token: token)
            .ConfigureAwait(false);
        await using var responseDisposable = response.ConfigureAwait(false);
        if (!response.Success) return [];

        using var ms = new MemoryStream();
        await response.Value.CopyToAsync(ms, token).ConfigureAwait(false);
        return ms.ToArray();
    }

    public async Task DeleteAttachmentAsync(AttachmentViewDto attachmentView,
        IAttachmentService.AttachmentServiceConfig config, CancellationToken token = default)
    {
        Config = config;

        var attachment = await attachmentRepository.GetAsync(attachmentView.Id, token: token).ConfigureAwait(false);
        attachment.SetDeleted((await userService.GetCurrentUserAsync().ConfigureAwait(false))?.Id);
        await attachmentRepository.UpdateAsync(attachment, token: token).ConfigureAwait(false);

        // Cancellation token in the `DeleteFileAsync` method is only used by the Azure Blob Storage implementation.
        await fileService.DeleteFileAsync(attachmentView.FileId, ExpandPath(attachmentView.FileId),
            token).ConfigureAwait(false);

        if (attachmentView.IsImage)
            await fileService.DeleteFileAsync(attachmentView.FileId, ExpandPath(attachmentView.FileId, thumbnail: true),
                token).ConfigureAwait(false);
    }

    public async Task<int> SaveAttachmentsAsync(int complaintId, List<IFormFile> files,
        IAttachmentService.AttachmentServiceConfig config, CancellationToken token = default)
    {
        if (files.Count == 0) return 0;

        Config = config;
        var complaint = await complaintRepository.GetAsync(complaintId, token: token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        var i = 0;

        foreach (var formFile in files.Where(formFile => formFile is { Length: > 0, FileName: not "" }))
        {
            var attachment = attachmentManager.Create(formFile, complaint, currentUser);
            attachment.IsImage = await SaveFileAsync(formFile, attachment.FileId).ConfigureAwait(false);
            await attachmentRepository.InsertAsync(attachment, autoSave: false, token: token).ConfigureAwait(false);
            i++;
        }

        await attachmentRepository.SaveChangesAsync(token).ConfigureAwait(false);
        return i;
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
        catch (Exception e)
        {
            // Log error but take no other action here.
            var customData = new Dictionary<string, object>
            {
                { "Action", "Saving Image" },
                { "IFormFile", formFile },
                { "File ID", fileId },
            };
            await errorLogger.LogErrorAsync(e, customData).ConfigureAwait(false);
            return false;
        }
    }

    private Task SaveThumbnailAsFileAsync(Image image, string fileId) =>
        SaveImageAsFileAsync(image, fileId, asThumbnail: true);

    private async Task SaveImageAsFileAsync(Image image, string fileId, bool asThumbnail = false)
    {
        var ms = new MemoryStream();
        await using var msDisposable = ms.ConfigureAwait(false);
        await image.SaveAsync(ms, image.Metadata.DecodedImageFormat!).ConfigureAwait(false);
        await fileService.SaveFileAsync(ms, fileId, ExpandPath(fileId, asThumbnail)).ConfigureAwait(false);
    }
}
