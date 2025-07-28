using System.Reflection;

namespace Cts.WebApp.Platform.Settings;

internal static partial class AppSettings
{
    public static WebApplicationBuilder BindAppSettings(this WebApplicationBuilder builder)
    {
        Version = GetVersion();
        
        builder.Configuration.GetSection(nameof(SupportSettings)).Bind(SupportSettings);
        builder.Configuration.GetSection(nameof(RaygunSettings)).Bind(RaygunSettings);
        OrgNotificationsApiUrl = builder.Configuration.GetValue<string>(nameof(OrgNotificationsApiUrl));

        // Dev settings should only be used in the development environment and when explicitly enabled.
        var devConfig = builder.Configuration.GetSection(nameof(DevSettings));
        var useDevConfig = builder.Environment.IsDevelopment() && devConfig.Exists() &&
                           Convert.ToBoolean(devConfig[nameof(DevSettings.UseDevSettings)]);

        if (useDevConfig) devConfig.Bind(DevSettings);
        else DevSettings = ProductionDefault;
        
        return builder;
    }

    private static string GetVersion()
    {
        var entryAssembly = Assembly.GetEntryAssembly();
        var segments = (entryAssembly?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion ?? entryAssembly?.GetName().Version?.ToString() ?? "").Split('+');
        return segments[0] + (segments.Length > 0 ? $"+{segments[1][..Math.Min(7, segments[1].Length)]}" : "");
    }
}
