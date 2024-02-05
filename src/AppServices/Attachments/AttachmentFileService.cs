using Cts.AppServices.ErrorLogging;
using Cts.Domain.Entities.Attachments;
using GaEpd.FileService;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Cts.AppServices.Attachments;

public class AttachmentFileService(
    string attachmentsFolder,
    string thumbnailsFolder,
    int thumbnailSize,
    IFileService fileService,
    IAttachmentManager attachmentManager,
    IErrorLogger errorLogger)
    : IAttachmentFileService
{
    public async Task<byte[]> GetAttachmentFileAsync(string fileId, bool getThumbnail)
    {
        await using var response = await fileService.TryGetFileAsync(fileId, ExpandPath(fileId, getThumbnail))
            .ConfigureAwait(false);
        if (!response.Success) return [];

        using var ms = new MemoryStream();
        await response.Value.CopyToAsync(ms).ConfigureAwait(false);
        return ms.ToArray();
    }

    public async Task DeleteAttachmentFileAsync(string fileId, bool isImage)
    {
        if (string.IsNullOrEmpty(fileId)) return;
        await fileService.DeleteFileAsync(fileId, ExpandPath(fileId)).ConfigureAwait(false);
        if (isImage) await fileService.DeleteFileAsync(fileId, ExpandPath(fileId, true)).ConfigureAwait(false);
    }

    public async Task<Attachment?> SaveAttachmentFileAsync(IFormFile formFile)
    {
        if (formFile.Length == 0 || string.IsNullOrWhiteSpace(formFile.FileName))
            return null;

        var fileName = Path.GetFileName(formFile.FileName).Trim();
        var attachment = attachmentManager.Create(fileName, formFile.Length);

        attachment.IsImage = await SaveFileAsync(formFile, attachment.FileId).ConfigureAwait(false);

        return attachment;
    }

    // ReSharper disable once ConvertIfStatementToReturnStatement
    public FilesValidationResult ValidateUploadedFiles(List<IFormFile> formFiles)
    {
        if (formFiles.Count > 10)
            return FilesValidationResult.TooMany;

        if (formFiles.Exists(file => !FileTypes.FileUploadAllowed(file.FileName)))
            return FilesValidationResult.WrongType;

        return FilesValidationResult.Valid;
    }

    private string ExpandPath(string fileId, bool thumbnail = false) =>
        $"{(thumbnail ? thumbnailsFolder : attachmentsFolder)}/{fileId[..2]}";

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
                .Resize(new ResizeOptions { Size = new Size(thumbnailSize), Mode = ResizeMode.Pad })
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
