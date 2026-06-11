using Cts.WebApp.Platform.Settings;

namespace Cts.WebApp.Platform.AppConfiguration;

internal static class WebOptimizer
{
    public static void AddWebOptimizer(this IServiceCollection services)
    {
        if (AppSettings.DevSettings.UseDevSettings)
        {
            services.AddWebOptimizer(
                minifyJavaScript: AppSettings.DevSettings.EnableWebOptimizer,
                minifyCss: AppSettings.DevSettings.EnableWebOptimizer);
        }
        else
        {
            services.AddWebOptimizer(minifyJavaScript: true, minifyCss: true);
        }
    }
}
