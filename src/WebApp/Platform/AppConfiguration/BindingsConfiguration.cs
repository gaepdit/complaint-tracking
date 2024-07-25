using Cts.WebApp.Platform.Settings;
using System.Reflection;

namespace Cts.WebApp.Platform.AppConfiguration;

public static class BindingsConfiguration
{
    public static void BindSettings(WebApplicationBuilder builder)
    {
        // App settings
        builder.Configuration.GetSection(nameof(AppSettings.SupportSettings))
            .Bind(AppSettings.SupportSettings);

        builder.Configuration.GetSection(nameof(AppSettings.RaygunSettings))
            .Bind(AppSettings.RaygunSettings);

        // App versioning
        var versionSegments = (Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion ?? Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "").Split('+');

        AppSettings.SupportSettings.InformationalVersion = versionSegments[0];
        if (versionSegments.Length > 0)
            AppSettings.SupportSettings.InformationalBuild = versionSegments[1][..Math.Min(7, versionSegments[1].Length)];

        // Dev settings
        var devConfig = builder.Configuration.GetSection(nameof(AppSettings.DevSettings));
        var useDevConfig = devConfig.Exists() &&
                           Convert.ToBoolean(devConfig[nameof(AppSettings.DevSettings.UseDevSettings)]);

        if (useDevConfig)
            devConfig.Bind(AppSettings.DevSettings);
        else
            AppSettings.DevSettings = AppSettings.ProductionDefault;
    }
}
