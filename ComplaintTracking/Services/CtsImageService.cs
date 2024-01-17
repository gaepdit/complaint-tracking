using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace ComplaintTracking.Services
{
    public class CtsImageService : ICtsImageService
    {
        private readonly IErrorLogger _errorLogger;

        public CtsImageService(IErrorLogger errorLogger)
        {
            _errorLogger = errorLogger;
        }

        public Task<bool> SaveThumbnailAsync(IFormFile file, string savePath) =>
            SaveImageAsync(file, savePath, true);

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

                await image.SaveAsync(savePath);
                return true;
            }
            catch (Exception ex)
            {
                // Log error but take no other action here
                var customData = new Dictionary<string, object>
                {
                    {"Action", "Saving Image"},
                    {"As Thumbnail", asThumbnail},
                    {"IFormFile", file},
                    {"Save Path", savePath}
                };
                await _errorLogger.LogErrorAsync(ex, "SaveImage", customData);
                return false;
            }
        }
    }

    public interface ICtsImageService
    {
        Task<bool> SaveThumbnailAsync(IFormFile file, string savePath);
        Task<bool> SaveImageAsync(IFormFile file, string savePath, bool asThumbnail = false);
    }
}
