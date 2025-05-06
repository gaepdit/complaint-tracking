using Cts.AppServices.IdentityServices;
using Cts.WebApp.Platform.Settings;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Identity.Web;
using Okta.AspNetCore;

namespace Cts.WebApp.Platform.AppConfiguration;

public static class AuthenticationServices
{
    public static void ConfigureAuthentication(this WebApplicationBuilder builder)
    {
        var authenticationBuilder = builder.Services
            .ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            })
            .AddAuthentication(options => options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie();

        if (AppSettings.DevSettings.UseExternalAuthentication)
        {
            authenticationBuilder

                // Requires an Okta account
                .AddOktaMvc(authenticationScheme: IdentityProviders.OktaScheme, new OktaMvcOptions
                {
                    OktaDomain = builder.Configuration.GetValue<string>("Okta:OktaDomain"),
                    AuthorizationServerId = builder.Configuration.GetValue<string>("Okta:AuthorizationServerId"),
                    ClientId = builder.Configuration.GetValue<string>("Okta:ClientId"),
                    ClientSecret = builder.Configuration.GetValue<string>("Okta:ClientSecret"),
                    Scope = new List<string> { "openid", "profile", "email" },
                })

                // Requires an Entra ID account
                // Note: `cookieScheme: null` is mandatory. See https://github.com/AzureAD/microsoft-identity-web/issues/133#issuecomment-739550416
                .AddMicrosoftIdentityWebApp(builder.Configuration, openIdConnectScheme: IdentityProviders.EntraIdScheme,
                    cookieScheme: null);
        }

        builder.Services.AddAuthorization();
    }
}
