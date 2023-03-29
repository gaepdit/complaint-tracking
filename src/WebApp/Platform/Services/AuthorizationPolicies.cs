using Cts.Domain.Security.Policies;

namespace Cts.WebApp.Platform.Services;

public static class AuthorizationPolicies
{
    public static void AddAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorization(opts =>
        {
            opts.AddPolicy(PolicyName.SiteMaintainer, Policies.SiteMaintainerPolicy());
            opts.AddPolicy(PolicyName.UserAdministrator, Policies.UserAdministratorPolicy());
            opts.AddPolicy(PolicyName.DivisionManager, Policies.DivisionManagerPolicy());
        });
    }
}
