using Cts.TestData;
using Cts.WebApp.Platform.Settings;
using GaEpd.FileService;

namespace Cts.WebApp.Platform.AppConfiguration;

public static class FileStorage
{
    public static async Task ConfigureFileStorage(this IHostApplicationBuilder builder)
    {
        builder.AddFileServices();
        if (AppSettings.DevSettings.UseDevSettings) await SeedFileStoreAsync(builder.Services);
    }

    // Initialize the attachment file store
    private static async Task SeedFileStoreAsync(IServiceCollection services)
    {
        var fileService = services.BuildServiceProvider().GetRequiredService<IFileService>();

        foreach (var attachment in AttachmentData.GetAttachmentFiles())
        {
            var fileBytes = attachment.Base64EncodedFile == null
                ? []
                : Convert.FromBase64String(attachment.Base64EncodedFile);

            await using var fileStream = new MemoryStream(fileBytes);
            await fileService.SaveFileAsync(fileStream, attachment.FileName, attachment.Path);
        }
    }
}
