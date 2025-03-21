using Cts.WebApp.Platform.Settings;
using JetBrains.Annotations;
using Microsoft.Extensions.Caching.Memory;

namespace Cts.WebApp.Platform.OrgNotifications;

// Organizational notifications

public static class OrgNotificationsServiceExtensions
{
    public static void AddOrgNotifications(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddScoped<IOrgNotifications, OrgNotifications>();
    }
}

public interface IOrgNotifications
{
    Task<List<OrgNotification>> GetOrgNotificationsAsync();
}

public record OrgNotification
{
    public required string Message { get; [UsedImplicitly] init; }
}

public class OrgNotifications(
    IHttpClientFactory httpClientFactory,
    IMemoryCache cache,
    ILogger<OrgNotifications> logger) : IOrgNotifications
{
    private const string ApiEndpoint = "/current";
    private const string CacheKey = nameof(OrgNotifications);

    public async Task<List<OrgNotification>> GetOrgNotificationsAsync()
    {
        if (AppSettings.OrgNotificationsApiUrl is null) return [];

        if (cache.TryGetValue(CacheKey, out List<OrgNotification>? notifications) && notifications != null)
            return notifications;

        try
        {
            notifications = await httpClientFactory.FetchApiDataAsync<List<OrgNotification>>(
                AppSettings.OrgNotificationsApiUrl, ApiEndpoint);
            if (notifications is null) return [];
            cache.Set(CacheKey, notifications, new TimeSpan(hours: 1, minutes: 0, seconds: 0));
        }
        catch (Exception ex)
        {
            // If the API is unresponsive or other error occurs, no notifications will be displayed.
            logger.LogError(ex, "Failed to fetch organizational notifications.");
            return [];
        }

        return notifications;
    }
}

public static class ApiExtensions
{
    public static async Task<T?> FetchApiDataAsync<T>(this IHttpClientFactory httpClientFactory,
        string apiUrl, string endpointPath)
    {
        using var client = httpClientFactory.CreateClient();
        using var response = await client.GetAsync(UriCombine(apiUrl, endpointPath));
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>();
    }

    private static string UriCombine(string a, string b, char separator = '/')
    {
        if (string.IsNullOrEmpty(a)) return b;
        if (string.IsNullOrEmpty(b)) return a;
        return a.TrimEnd(separator) + separator + b.TrimStart(separator);
    }
}
