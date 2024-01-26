using ComplaintTracking.Models;
using GaEpd.FileService;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace ComplaintTracking.Services
{
    public class CtsAttachmentService(IFileService fileService, IErrorLogger errorLogger) : ICtsAttachmentService
    {
        public async Task<byte[]> GetAttachmentAsync(string fileId, bool getThumbnail)
        {
            await using var response = await fileService.TryGetFileAsync(fileId, ExpandPath(fileId, getThumbnail));
            if (!response.Success) return [];

            using var ms = new MemoryStream();
            await response.Value.CopyToAsync(ms);
            return ms.ToArray();
        }

        public async Task DeleteAttachmentAsync(string fileId, bool isImage)
        {
            if (string.IsNullOrEmpty(fileId)) return;
            await fileService.DeleteFileAsync(fileId, ExpandPath(fileId));
            if (isImage) await fileService.DeleteFileAsync(fileId, ExpandPath(fileId, true));
        }

        public async Task<Attachment> SaveAttachmentAsync(IFormFile formFile)
        {
            if (formFile.Length == 0 || string.IsNullOrWhiteSpace(formFile.FileName))
                return null;

            var fileName = Path.GetFileName(formFile.FileName).Trim();
            var fileExtension = Path.GetExtension(fileName);
            var attachmentId = Guid.NewGuid();
            var fileId = $"{attachmentId}{fileExtension}";

            var isImage = await SaveFileAsync(formFile, fileId);

            return new Attachment
            {
                Id = attachmentId,
                FileName = fileName,
                FileExtension = fileExtension,
                DateUploaded = DateTime.Now,
                Size = formFile.Length,
                IsImage = isImage,
            };
        }

        // SaveFileAsync returns true if formFile is an image; otherwise false.
        private async Task<bool> SaveFileAsync(IFormFile formFile, string fileId)
        {
            // Try to save using the image service (which handles image rotation and thumbnail generation).
            // If successful, file is an image type.
            if (await TrySaveImageAsync(formFile, fileId)) return true;

            // If image service fails, save file directly. File is not an image type.
            await fileService.SaveFileAsync(formFile.OpenReadStream(), fileId, ExpandPath(fileId));
            return false;
        }


        private async Task<bool> TrySaveImageAsync(IFormFile formFile, string fileId)
        {
            if (!FileTypes.FilenameImpliesImage(formFile.FileName.Trim())) return false;

            try
            {
                using var image = await Image.LoadAsync(formFile.OpenReadStream());

                // Save full size image.
                await SaveImageAsFileAsync(image, fileId);

                // Save thumbnail.
                var imageThumbnail = image.Clone(x => x
                    .Resize(new ResizeOptions { Size = new Size(CTS.ThumbnailSize), Mode = ResizeMode.Pad })
                    .BackgroundColor(Color.White));
                await SaveThumbnailAsFileAsync(imageThumbnail, fileId);

                return true;
            }
            catch (Exception ex)
            {
                // Log error but take no other action here
                var customData = new Dictionary<string, object>
                {
                    { "Action", "Saving Image" },
                    { "IFormFile", formFile },
                    { "Save Path", fileId }
                };
                await errorLogger.LogErrorAsync(ex, "TrySaveImageAsync", customData);
                return false;
            }
        }

        private Task SaveThumbnailAsFileAsync(Image image, string fileId) => SaveImageAsFileAsync(image, fileId, true);

        private async Task SaveImageAsFileAsync(Image image, string fileId, bool asThumbnail = false)
        {
            await using var ms = new MemoryStream();
            await image.SaveAsync(ms, image.Metadata.DecodedImageFormat!);
            await fileService.SaveFileAsync(ms, fileId, ExpandPath(fileId, asThumbnail));
        }

        private static string ExpandPath(string fileId, bool thumbnail = false) =>
            $"{(thumbnail ? FilePaths.ThumbnailsFolder : FilePaths.AttachmentsFolder)}/{fileId[..2]}";

        // ReSharper disable once ConvertIfStatementToReturnStatement
        public FilesValidationResult ValidateUploadedFiles(List<IFormFile> formFiles)
        {
            if (formFiles.Count > 10)
                return FilesValidationResult.TooMany;

            if (formFiles.Exists(file => file.Length > 0 && !FileTypes.FileUploadAllowed(file.FileName)))
                return FilesValidationResult.WrongType;

            return FilesValidationResult.Valid;
        }
    }

    public enum FilesValidationResult
    {
        Valid,
        TooMany,
        WrongType
    }

    public interface ICtsAttachmentService
    {
        Task<byte[]> GetAttachmentAsync(string fileId, bool getThumbnail);
        Task DeleteAttachmentAsync(string fileId, bool isImage);
        Task<Attachment> SaveAttachmentAsync(IFormFile formFile);
        FilesValidationResult ValidateUploadedFiles(List<IFormFile> formFiles);
    }
}
