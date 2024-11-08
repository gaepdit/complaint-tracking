using System.Collections;
using Cts.AppServices.ErrorLogging;
using Cts.WebApp.Platform.Settings;
using GaEpd.FileService;
using Mindscape.Raygun4Net.AspNetCore;

namespace Cts.WebApp.Platform.ErrorLogging;

public class ErrorLogger(IFileService fileService, IServiceProvider serviceProvider) : IErrorLogger
{
    public Task<string> LogErrorAsync(Exception exception, string context = "")
    {
        var customData = new Dictionary<string, object>();
        if (!string.IsNullOrEmpty(context)) customData.Add("Context", context);
        return LogErrorAsync(exception, customData);
    }

    public async Task<string> LogErrorAsync(Exception exception, Dictionary<string, object> customData)
    {
        var shortId = ShortId.GetShortId();
        customData.Add("CTS Error ID", shortId);

        if (!string.IsNullOrEmpty(AppSettings.RaygunSettings.ApiKey))
        {
            await LogRaygunErrorAsync(exception, customData);
            return shortId;
        }

        using var ms = new MemoryStream();
        await using var sw = new StreamWriter(ms);

        await sw.WriteLineAsync($"Date: {DateTime.Now}");
        foreach (var pair in customData) await sw.WriteLineAsync($"{pair.Key}: {pair.Value}");

        var depth = 0;
        while (depth < 4)
        {
            await sw.WriteLineAsync(exception.GetType().FullName);
            await sw.WriteLineAsync("Message : " + exception.Message);
            await sw.WriteLineAsync("StackTrace : " + exception.StackTrace);

            if (exception.InnerException is null) break;

            depth += 1;
            exception = exception.InnerException;
        }

        await sw.FlushAsync();
        await fileService.SaveFileAsync(ms, $"{shortId}.txt", "Errors");
        return shortId;
    }

    private Task LogRaygunErrorAsync(Exception exception, IDictionary customData) => 
        serviceProvider.GetService<RaygunClient>()!.SendInBackground(exception, null, customData);
}
