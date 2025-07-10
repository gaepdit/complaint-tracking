using Cts.WebApp.Platform.Settings;

namespace Cts.WebApp.Platform.AppConfiguration;

internal static class WebOptimizer
{
    public static void AddWebOptimizer(this IHostApplicationBuilder builder)
    {
        if (builder.Environment.IsDevelopment())
        {
            builder.Services.AddWebOptimizer(
                minifyJavaScript: AppSettings.DevSettings.EnableWebOptimizerInDev,
                minifyCss: AppSettings.DevSettings.EnableWebOptimizerInDev);
        }
        else
        {
            builder.Services.AddWebOptimizer();
        }
    }
}
