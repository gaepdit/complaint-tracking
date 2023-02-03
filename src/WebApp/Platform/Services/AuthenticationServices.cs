using Cts.WebApp.Platform.Settings;
using Microsoft.Identity.Web;

namespace Cts.WebApp.Platform.Services;

public static class AuthenticationServices
{
    public static void AddAuthenticationServices(this IServiceCollection services,
        ConfigurationManager configuration,
        bool isLocal)
    {
        var authenticationBuilder = services.AddAuthentication();

        // An Azure AD app must be registered and configured in the settings file.
        if (!isLocal || ApplicationSettings.LocalDevSettings.UseAzureAd)
            authenticationBuilder.AddMicrosoftIdentityWebApp(configuration, cookieScheme: null);
        // Note: `cookieScheme: null` is mandatory. See https://github.com/AzureAD/microsoft-identity-web/issues/133#issuecomment-739550416
    }
}
