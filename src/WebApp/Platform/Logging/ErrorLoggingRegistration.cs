using Cts.AppServices.ErrorLogging;
using Cts.WebApp.Platform.Settings;
using Mindscape.Raygun4Net;
using Mindscape.Raygun4Net.AspNetCore;

namespace Cts.WebApp.Platform.Logging;

public static class ErrorLoggingRegistration
{
    public static void ConfigureErrorLogging(this IServiceCollection services, string environmentName) => services
        .AddTransient<IErrorLogger, ErrorLogger>()
        .AddSingleton(provider =>
        {
            var client = new RaygunClient(provider.GetService<RaygunSettings>()!,
                provider.GetService<IRaygunUserProvider>()!);
            client.SendingMessage += (_, eventArgs) => eventArgs.Message.Details.Tags.Add(environmentName);
            return client;
        })
        .AddRaygun(opts =>
        {
            opts.ApiKey = AppSettings.RaygunSettings.ApiKey;
            opts.ApplicationVersion = AppSettings.SupportSettings.InformationalVersion;
            opts.IgnoreFormFieldNames = ["*Password"];
            opts.EnvironmentVariables.Add("ASPNETCORE_*");
        })
        .AddRaygunUserProvider()
        .AddHttpContextAccessor(); // needed by RaygunScriptPartial
}
