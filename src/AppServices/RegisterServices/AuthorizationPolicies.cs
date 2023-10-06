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
            opts.AddPolicy(nameof(Policies.ActiveUser), Policies.ActiveUser);
            opts.AddPolicy(nameof(Policies.AdministrationView), Policies.AdministrationView);
            opts.AddPolicy(nameof(Policies.DivisionManager), Policies.DivisionManager);
            opts.AddPolicy(nameof(Policies.LoggedInUser), Policies.LoggedInUser);
            opts.AddPolicy(nameof(Policies.SiteMaintainer), Policies.SiteMaintainer);
            opts.AddPolicy(nameof(Policies.StaffUser), Policies.StaffUser);
            opts.AddPolicy(nameof(Policies.UserAdministrator), Policies.UserAdministrator);
        });

        services.AddSingleton<IAuthorizationHandler, ComplaintViewPermissionsHandler>();
        services.AddSingleton<IAuthorizationHandler, ComplaintUpdatePermissionsHandler>();
    }
}
