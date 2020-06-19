using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComplaintTracking.Services
{
    public class ImageService : IImageService
    {
        private readonly IErrorLogger _errorLogger;
        public ImageService(IErrorLogger errorLogger) { _errorLogger = errorLogger; }

        public async Task<bool> SaveThumbnail(IFormFile file, string savePath)
        {
            return await SaveImage(file, savePath, true);
        }

        public async Task<bool> SaveImage(IFormFile file, string savePath, bool asThumbnail = false)
        {
            if (!FileTypes.FilenameImpliesImage(file.FileName.Trim()))
                return false;

            try
            {
                using var image = Image.Load(file.OpenReadStream());
                if (image == null) return false;

                if (asThumbnail)
                {
                    image.Mutate(x => x.AutoOrient()
                        .Resize(new ResizeOptions
                        {
                            Size = new Size(CTS.ThumbnailSize),
                            Mode = ResizeMode.Pad
                        })
                        .BackgroundColor(Rgba32.White));
                }
                else
                {
                    image.Mutate(x => x.AutoOrient());
                }

                if (image == null) return false;

                image.Save(savePath);
                return true;
            }
            catch (Exception ex)
            {
                // Log error but take no other action here
                var customData = new Dictionary<string, object>();
                customData.Add("Action", "Saving Image");
                customData.Add("As Thumbnail", asThumbnail);
                customData.Add("IFormFile", file);
                customData.Add("Save Path", savePath);
                await _errorLogger.LogErrorAsync(ex, "SaveImage", customData);
                return false;
            }
        }
    }

    public interface IImageService
    {
        Task<bool> SaveThumbnail(IFormFile file, string savePath);
        Task<bool> SaveImage(IFormFile file, string savePath, bool asThumbnail = false);
    }
}
