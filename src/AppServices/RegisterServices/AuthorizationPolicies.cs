using Cts.AppServices.Complaints.Permissions;
using Cts.AppServices.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Cts.AppServices.RegisterServices;

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

        services.AddSingleton<IAuthorizationHandler>(_ => new ComplaintViewPermissionsHandler());
    }
}
