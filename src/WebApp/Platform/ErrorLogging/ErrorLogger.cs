using Cts.AppServices.ErrorLogging;
using Microsoft.Extensions.Options;
using Mindscape.Raygun4Net.AspNetCore;
using System.Collections;

namespace Cts.WebApp.Platform.ErrorLogging;

public class ErrorLogger(
    IRaygunAspNetCoreClientProvider clientProvider,
    IOptions<RaygunSettings> settings,
    IHttpContextAccessor httpContextAccessor)
    : IErrorLogger
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
        await LogRaygunErrorAsync(exception, customData);
        return shortId;
    }

    private Task LogRaygunErrorAsync(Exception exception, IDictionary customData) =>
        clientProvider.GetClient(settings.Value, httpContextAccessor.HttpContext)
            .SendInBackground(exception, null, customData);
}
