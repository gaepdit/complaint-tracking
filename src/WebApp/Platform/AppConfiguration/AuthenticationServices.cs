using Cts.AppServices.AuthenticationServices;
using Cts.AppServices.AuthorizationPolicies;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Cts.WebApp.Platform.AppConfiguration;

public static class AuthenticationServices
{
    public static void ConfigureAuthentication(this IHostApplicationBuilder builder)
    {
        var authenticationBuilder = builder.Services
            .ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            })
            .AddAuthentication(options => options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie();

        var configuration = builder.Configuration;

        if (configuration.LoginProviderNames().Contains(LoginProviders.DuoScheme))
        {
            authenticationBuilder.AddOpenIdConnect(authenticationScheme: LoginProviders.DuoScheme,
                displayName: "Duo OIDC",
                configureOptions: options =>
                {
                    var configSection = builder.Configuration.GetSection("DuoSecurity");

                    options.Authority = configSection["Authority"];
                    options.ClientId = configSection["ClientId"];
                    options.ClientSecret = configSection["ClientSecret"];

                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.Scope.Add("email");

                    // `SignInScheme = null` is mandatory. See https://github.com/AzureAD/microsoft-identity-web/issues/133#issuecomment-739550416
                    options.SignInScheme = null;
                    options.ResponseType = OpenIdConnectResponseType.Code;
                    options.MapInboundClaims = false;
                    // Each OIDC provider must have a unique callback path. "/signin-oidc" is already used by Azure Entra ID.
                    options.CallbackPath = "/signin-oidc-duo";
                });
        }

        if (configuration.LoginProviderNames().Contains(LoginProviders.EntraIdScheme))
        {
            // Requires an Entra ID account
            authenticationBuilder.AddMicrosoftIdentityWebApp(configuration,
                openIdConnectScheme: LoginProviders.EntraIdScheme, cookieScheme: null);
            // Note: `cookieScheme: null` is mandatory. See https://github.com/AzureAD/microsoft-identity-web/issues/133#issuecomment-739550416
        }

        builder.Services
            .AddAuthenticationAppServices()
            .AddAuthorizationPolicies()
            .AddAuthorization();
    }
}
