using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComplaintTracking.Services
{
    public class ImageService : IImageService
    {
        private readonly IErrorLogger _errorLogger;
        public ImageService(IErrorLogger errorLogger) { _errorLogger = errorLogger; }

        public async Task<bool> SaveThumbnailAsync(IFormFile file, string savePath)
        {
            return await SaveImageAsync(file, savePath, true);
        }

        public async Task<bool> SaveImageAsync(IFormFile file, string savePath, bool asThumbnail = false)
        {
            if (!FileTypes.FilenameImpliesImage(file.FileName.Trim()))
                return false;

            try
            {
                using var image = await Image.LoadAsync(file.OpenReadStream());
                if (image == null) return false;

                if (asThumbnail)
                {
                    image.Mutate(x => x.AutoOrient()
                        .Resize(new ResizeOptions
                        {
                            Size = new Size(CTS.ThumbnailSize),
                            Mode = ResizeMode.Pad
                        })
                        .BackgroundColor(Color.White));
                }
                else
                {
                    image.Mutate(x => x.AutoOrient());
                }

                if (image == null) return false;

                await image.SaveAsync(savePath);
                return true;
            }
            catch (Exception ex)
            {
                // Log error but take no other action here
                var customData = new Dictionary<string, object>
                {
                    { "Action", "Saving Image" },
                    { "As Thumbnail", asThumbnail },
                    { "IFormFile", file },
                    { "Save Path", savePath }
                };
                await _errorLogger.LogErrorAsync(ex, "SaveImage", customData);
                return false;
            }
        }
    }

    public interface IImageService
    {
        Task<bool> SaveThumbnailAsync(IFormFile file, string savePath);
        Task<bool> SaveImageAsync(IFormFile file, string savePath, bool asThumbnail = false);
    }
}
