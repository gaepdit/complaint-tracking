using Cts.WebApp.Platform.Settings;

namespace Cts.WebApp.Platform.Services;

public static class AppConfiguration
{
    public static void BindSettings(WebApplicationBuilder builder)
    {
        builder.Configuration.GetSection(nameof(ApplicationSettings.RaygunSettings))
            .Bind(ApplicationSettings.RaygunSettings);

        var devConfig = builder.Configuration.GetSection(nameof(ApplicationSettings.DevSettings));
        var useDevConfig = devConfig.Exists() &&
            Convert.ToBoolean(devConfig[nameof(ApplicationSettings.DevSettings.UseDevSettings)]);

        if (useDevConfig)
            devConfig.Bind(ApplicationSettings.DevSettings);
        else
            ApplicationSettings.DevSettings = ApplicationSettings.ProductionDefault;
    }
}
