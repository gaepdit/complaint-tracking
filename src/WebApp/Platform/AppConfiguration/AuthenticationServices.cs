using Cts.WebApp.Platform.Settings;
using Microsoft.Identity.Web;

namespace Cts.WebApp.Platform.AppConfiguration;

public static class AuthenticationServices
{
    public static void AddAuthenticationServices(this WebApplicationBuilder builder)
    {
        var authenticationBuilder = builder.Services.AddAuthentication();

        // An Azure AD app must be registered and configured in the settings file.
        if (AppSettings.DevSettings.UseAzureAd)
            authenticationBuilder.AddMicrosoftIdentityWebApp(builder.Configuration, cookieScheme: null);
        // Note: `cookieScheme: null` is mandatory. See https://github.com/AzureAD/microsoft-identity-web/issues/133#issuecomment-739550416
    }
}
