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
            await using var response = await fileService.TryGetFileAsync(fileId, getThumbnail ? FilePaths.ThumbnailsFolder : FilePaths.AttachmentsFolder);
            if (!response.Success) return [];

            using var ms = new MemoryStream();
            await response.Value.CopyToAsync(ms);
            return ms.ToArray();
        }

        public async Task DeleteAttachmentAsync(string fileId, bool isImage)
        {
            if (string.IsNullOrEmpty(fileId)) return;
            await fileService.DeleteFileAsync(fileId, FilePaths.AttachmentsFolder);
            if (isImage) await fileService.DeleteFileAsync(fileId, FilePaths.ThumbnailsFolder);
        }

        public async Task<Attachment> SaveAttachmentAsync(IFormFile formFile)
        {
            if (formFile.Length == 0 || string.IsNullOrWhiteSpace(formFile.FileName))
                return null;

            var fileName = Path.GetFileName(formFile.FileName).Trim();
            var fileExtension = Path.GetExtension(fileName);
            var attachmentId = Guid.NewGuid();
            var fileId = string.Concat(attachmentId.ToString(), fileExtension);
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
            await fileService.SaveFileAsync(formFile.OpenReadStream(), fileId, FilePaths.AttachmentsFolder);
            return false;
        }

        private async Task<bool> TrySaveImageAsync(IFormFile formFile, string fileId)
        {
            if (!FileTypes.FilenameImpliesImage(formFile.FileName.Trim())) return false;

            try
            {
                using var image = await Image.LoadAsync(formFile.OpenReadStream());
                if (image == null) return false;

                // Save full size image.
                await SaveImageFileAsync(image, fileId, FilePaths.AttachmentsFolder);

                // Save thumbnail.
                var imageThumbnail = image.Clone(x => x
                    .Resize(new ResizeOptions { Size = new Size(CTS.ThumbnailSize), Mode = ResizeMode.Pad })
                    .BackgroundColor(Color.White));
                await SaveImageFileAsync(imageThumbnail, fileId, FilePaths.ThumbnailsFolder);

                return true;
            }
            catch (Exception ex)
            {
                // Log error but take no other action here
                var customData = new Dictionary<string, object>
                {
                    {"Action", "Saving Image"},
                    {"IFormFile", formFile},
                    {"Save Path", fileId}
                };
                await errorLogger.LogErrorAsync(ex, "TrySaveImageAsync", customData);
                return false;
            }
        }

        private async Task SaveImageFileAsync(Image image, string fileId, string path)
        {
            await using var ms = new MemoryStream();
            await image.SaveAsync(ms, image.Metadata.DecodedImageFormat);
            await fileService.SaveFileAsync(ms, fileId, path);
        }

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
