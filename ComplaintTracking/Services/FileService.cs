using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ComplaintTracking.Models;
using Microsoft.AspNetCore.Http;

namespace ComplaintTracking.Services
{
    public class FileService : IFileService
    {
        private readonly IErrorLogger _errorLogger;
        private readonly IImageService _imageService;

        public FileService(IErrorLogger errorLogger, IImageService imageService)
        {
            _errorLogger = errorLogger;
            _imageService = imageService;
        }

        public async Task TryDeleteFileAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return;
            try
            {
                File.Delete(filePath);
            }
            catch (Exception ex)
            {
                // Log error but take no other action here
                var customData = new Dictionary<string, object>
                {
                    {"Action", "Deleting File"},
                    {"File", filePath}
                };
                await _errorLogger.LogErrorAsync(ex, "TryDeleteFileAsync", customData);
            }
        }

        public async Task SaveFileAsync(IFormFile file, string savePath)
        {
            // Try to save using the image service (which handles image rotation problems). 
            // If image service fails, save file directly.
            if (!await _imageService.SaveImageAsync(file, savePath))
            {
                await using var stream = new FileStream(savePath, FileMode.Create);
                await file.CopyToAsync(stream);
            }
        }

        public async Task<Attachment> SaveAttachmentAsync(IFormFile file)
        {
            if (file.Length == 0 || string.IsNullOrWhiteSpace(file.FileName))
            {
                return null;
            }

            var fileName = file.FileName.Trim();
            var fileExtension = Path.GetExtension(fileName);
            var isImage = false;
            var attachmentId = Guid.NewGuid();

            if (FileTypes.FilenameImpliesImage(fileName))
            {
                var thumbnailSavePath = Path.Combine(FilePaths.ThumbnailsFolder,
                    string.Concat(attachmentId.ToString(), fileExtension));
                if (await _imageService.SaveThumbnailAsync(file, thumbnailSavePath))
                {
                    isImage = true;
                }
            }

            var savePath = Path.Combine(FilePaths.AttachmentsFolder,
                string.Concat(attachmentId.ToString(), fileExtension));
            await SaveFileAsync(file, savePath);

            return new Attachment
            {
                Id = attachmentId,
                FileName = Path.GetFileName(fileName),
                FileExtension = fileExtension,
                DateUploaded = DateTime.Now,
                Size = file.Length,
                IsImage = isImage,
            };
        }

        public FilesValidationResult ValidateUploadedFiles(List<IFormFile> files)
        {
            if (files.Count > 10)
            {
                return FilesValidationResult.TooMany;
            }

            if (files.Any(file => file.Length > 0 && !FileTypes.FileUploadAllowed(file.FileName)))
            {
                return FilesValidationResult.WrongType;
            }

            return FilesValidationResult.Valid;
        }
    }

    public enum FilesValidationResult
    {
        Valid,
        TooMany,
        WrongType
    }

    public interface IFileService
    {
        Task TryDeleteFileAsync(string filePath);
        Task SaveFileAsync(IFormFile file, string savePath);
        Task<Attachment> SaveAttachmentAsync(IFormFile file);
        FilesValidationResult ValidateUploadedFiles(List<IFormFile> files);
    }
}
