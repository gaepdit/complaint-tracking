using Azure.Identity;
using System.Reflection;

namespace Cts.WebApp.Platform.Settings;

internal static partial class AppSettings
{
    private static void AddKeyVault(this IHostApplicationBuilder builder)
    {
        var keyVaultName = builder.Configuration.GetValue<string>("AzureKeyVaultName");
        if (string.IsNullOrEmpty(keyVaultName)) return;

        var keyVaultUri = new Uri($"https://{keyVaultName}.vault.azure.net");
        builder.Configuration.AddAzureKeyVault(keyVaultUri, new DefaultAzureCredential());
    }

    public static void LoadAppSettings(this IHostApplicationBuilder builder)
    {
        // Set default timeout for regular expressions.
        // https://learn.microsoft.com/en-us/dotnet/standard/base-types/best-practices#use-time-out-values
        AppDomain.CurrentDomain.SetData("REGEX_DEFAULT_MATCH_TIMEOUT", TimeSpan.FromMilliseconds(100));

        Version = GetVersion();

        builder.AddKeyVault();
        builder.LoadDevAppSettings();

        builder.Configuration.GetSection(nameof(Support)).Bind(Support);
        builder.Configuration.GetSection(nameof(DataDogSettings)).Bind(DataDogSettings);
        OrgNotificationsApiUrl = builder.Configuration.GetValue<string>(nameof(OrgNotificationsApiUrl));
    }

    private static string GetVersion()
    {
        var entryAssembly = Assembly.GetEntryAssembly();
        var segments = (entryAssembly?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion ?? entryAssembly?.GetName().Version?.ToString() ?? "").Split('+');
        return segments[0] + (segments.Length > 0 ? $"+{segments[1][..Math.Min(7, segments[1].Length)]}" : "");
    }
}
