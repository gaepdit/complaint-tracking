using Cts.WebApp.Platform.Settings;

namespace Cts.WebApp.Platform.Services;

public static class AppConfiguration
{
    public static void BindSettings(WebApplicationBuilder builder)
    {
        builder.Configuration.GetSection(nameof(ApplicationSettings.RaygunSettings))
            .Bind(ApplicationSettings.RaygunSettings);

        if (builder.Environment.IsDevelopment())
        {
            builder.Configuration.GetSection(nameof(ApplicationSettings.DevSettings))
                .Bind(ApplicationSettings.DevSettings);
        }
        else
        {
            ApplicationSettings.DevSettings = ApplicationSettings.ProductionDefault;
        }
    }
}
