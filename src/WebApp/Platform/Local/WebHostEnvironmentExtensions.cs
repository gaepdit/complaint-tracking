namespace Cts.WebApp.Platform.Local;

internal static class WebHostEnvironmentExtensions
{
    internal static bool IsLocalEnv(this IWebHostEnvironment environment) =>
        environment.IsEnvironment("Local");
}
