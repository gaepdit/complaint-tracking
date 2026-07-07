using Cts.AppServices.AuthorizationPolicies;

namespace Cts.WebApp.Platform.AppConfiguration;

public static class AuthorizationServices
{
    public static void ConfigureAuthorization(this IHostApplicationBuilder builder)
    {
        builder.Services
            .AddAuthorizationPolicies()
            .AddAuthorization();
    }
}
