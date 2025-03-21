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
    Task<List<OrgNotification>> FetchOrgNotificationsAsync();
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
    public async Task<List<OrgNotification>> FetchOrgNotificationsAsync()
    {
        if (AppSettings.OrgNotificationsApiUrl is null) return [];

        if (!cache.TryGetValue(nameof(OrgNotifications), out List<OrgNotification>? notifications))
        {
            notifications = await GetNotificationsFromApiAsync();
            cache.Set(nameof(OrgNotifications), notifications, new TimeSpan(hours: 1, minutes: 0, seconds: 0));
        }

        return notifications ?? [];
    }

    private async Task<List<OrgNotification>> GetNotificationsFromApiAsync()
    {
        using var client = httpClientFactory.CreateClient();
        try
        {
            var response = await client.GetAsync(AppSettings.OrgNotificationsApiUrl);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<OrgNotification>>() ?? [];
        }
        catch (Exception ex)
        {
            // If the API is unresponsive or other error occurs, no notifications will be displayed.
            logger.LogError(ex, "Failed to fetch organizational notifications.");
            return [];
        }
    }
}
