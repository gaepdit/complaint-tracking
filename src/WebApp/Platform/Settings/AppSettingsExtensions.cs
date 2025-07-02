namespace Cts.WebApp.Platform.Settings;

public static class AppSettingsExtensions
{
    public static void BindAppSettings(this WebApplicationBuilder builder)
    {
        builder.Configuration.GetSection(nameof(AppSettings.SupportSettings))
            .Bind(AppSettings.SupportSettings);
        builder.Configuration.GetSection(nameof(AppSettings.RaygunSettings))
            .Bind(AppSettings.RaygunSettings);

        // Organizational notifications
        AppSettings.OrgNotificationsApiUrl =
            builder.Configuration.GetValue<string>(nameof(AppSettings.OrgNotificationsApiUrl));

        // Dev settings should only be used in the development environment and when explicitly enabled.
        var devConfig = builder.Configuration.GetSection(nameof(AppSettings.DevSettings));
        var useDevConfig = builder.Environment.IsDevelopment() && devConfig.Exists() &&
                           Convert.ToBoolean(devConfig[nameof(AppSettings.DevSettings.UseDevSettings)]);

        if (useDevConfig) devConfig.Bind(AppSettings.DevSettings);
        else AppSettings.DevSettings = AppSettings.ProductionDefault;
    }
}
