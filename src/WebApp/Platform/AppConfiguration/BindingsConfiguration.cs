using Cts.WebApp.Platform.Settings;
using System.Reflection;

namespace Cts.WebApp.Platform.AppConfiguration;

public static class BindingsConfiguration
{
    public static void BindSettings(WebApplicationBuilder builder)
    {
        // Bind app settings.
        builder.Configuration.GetSection(nameof(AppSettings.SupportSettings))
            .Bind(AppSettings.SupportSettings);

        builder.Configuration.GetSection(nameof(AppSettings.RaygunSettings))
            .Bind(AppSettings.RaygunSettings);

        // Organizational notifications
        AppSettings.OrgNotificationsApiUrl =
            builder.Configuration.GetValue<string>(nameof(AppSettings.OrgNotificationsApiUrl));

        // Set app version.
        var segments = (Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion ?? Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "").Split('+');

        AppSettings.SupportSettings.InformationalVersion = segments[0];
        if (segments.Length > 0)
            AppSettings.SupportSettings.InformationalBuild = segments[1][..Math.Min(7, segments[1].Length)];

        // Dev settings should only be used in development environment and when explicitly enabled.
        var devConfig = builder.Configuration.GetSection(nameof(AppSettings.DevSettings));
        var useDevConfig = builder.Environment.IsDevelopment() && devConfig.Exists() &&
                           Convert.ToBoolean(devConfig[nameof(AppSettings.DevSettings.UseDevSettings)]);

        if (useDevConfig)
            devConfig.Bind(AppSettings.DevSettings);
        else
            AppSettings.DevSettings = AppSettings.ProductionDefault;
    }
}
