using Cts.AppServices.AuthenticationServices;
using Cts.AppServices.AuthorizationPolicies;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Identity.Web;
using Okta.AspNetCore;

namespace Cts.WebApp.Platform.AppConfiguration;

public static class AuthenticationServices
{
    public static void ConfigureAuthentication(this IServiceCollection services,
        IConfiguration configuration)
    {
        var authenticationBuilder = services
            .ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            })
            .AddAuthentication(options => options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie();

        if (configuration.LoginProviderNames().Contains(LoginProviders.OktaScheme))
        {
            // Requires an Okta account
            authenticationBuilder.AddOktaMvc(authenticationScheme: LoginProviders.OktaScheme, new OktaMvcOptions
            {
                OktaDomain = configuration.GetValue<string>("Okta:OktaDomain"),
                AuthorizationServerId = configuration.GetValue<string>("Okta:AuthorizationServerId"),
                ClientId = configuration.GetValue<string>("Okta:ClientId"),
                ClientSecret = configuration.GetValue<string>("Okta:ClientSecret"),
                Scope = new List<string> { "openid", "profile", "email" },
            });
        }

        if (configuration.LoginProviderNames().Contains(LoginProviders.EntraIdScheme))
        {
            // Requires an Entra ID account
            authenticationBuilder.AddMicrosoftIdentityWebApp(configuration,
                openIdConnectScheme: LoginProviders.EntraIdScheme, cookieScheme: null);
            // Note: `cookieScheme: null` is mandatory. See https://github.com/AzureAD/microsoft-identity-web/issues/133#issuecomment-739550416
        }

        services
            .AddAuthenticationServices()
            .AddAuthorizationPolicies()
            .AddAuthorization();
    }
}
