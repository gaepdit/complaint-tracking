using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace MyAppRoot.WebApp.Platform.Services;

public static class AuthenticationServices
{
    public static void AddAuthenticationServices(this IServiceCollection services,
        ConfigurationManager configuration,
        bool isLocal)
    {
        var authenticationBuilder = services.AddAuthentication();
        if (isLocal) return;

        // When running on the server, requires an Azure AD login account (configured in the app settings file).
        // (AddAzureAD is marked as obsolete and will be removed in a future version, but
        // the replacement, Microsoft Identity Web, is net yet compatible with RoleManager.)
        // Follow along at https://github.com/AzureAD/microsoft-identity-web/issues/1091
#pragma warning disable 618
        authenticationBuilder.AddAzureAD(opts =>
        {
            configuration.Bind(AzureADDefaults.AuthenticationScheme, opts);
            opts.CallbackPath = "/signin-oidc";
            opts.CookieSchemeName = "Identity.External";
        });
        services.Configure<OpenIdConnectOptions>(AzureADDefaults.OpenIdScheme, opts =>
        {
            opts.Authority += "/v2.0/";
            opts.TokenValidationParameters.ValidateIssuer = true;
            opts.UsePkce = true;
        });
#pragma warning restore 618
    }
}
