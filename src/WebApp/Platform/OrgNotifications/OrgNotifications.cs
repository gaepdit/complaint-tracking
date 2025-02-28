using Cts.WebApp.Platform.Settings;
using JetBrains.Annotations;

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
public class OrgNotifications(IHttpClientFactory httpClientFactory, ILogger<OrgNotifications> logger) : IOrgNotifications
{
    public async Task<List<OrgNotification>> FetchOrgNotificationsAsync()
    {
        if (AppSettings.OrgNotificationsApiUrl is null) return [];

        using var client = httpClientFactory.CreateClient();
        try
        {
            var response = await client.GetAsync(AppSettings.OrgNotificationsApiUrl);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<OrgNotification>>() ?? [];
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to fetch organizational notifications.");
            // If the API is unresponsive or other error occurs, no notifications will be displayed.
            return [];
        }
    }
}

public record OrgNotification
{
    public required string Message { get; [UsedImplicitly] init; }
}
