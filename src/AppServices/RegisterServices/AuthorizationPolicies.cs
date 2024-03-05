using Cts.AppServices.Complaints.Permissions;
using Cts.AppServices.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Cts.AppServices.RegisterServices;

public static class AuthorizationPolicies
{
    public static void AddAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
            .AddPolicy(nameof(Policies.ActiveUser), Policies.ActiveUser)
            .AddPolicy(nameof(Policies.DataExporter), Policies.DataExporter)
            .AddPolicy(nameof(Policies.DivisionManager), Policies.DivisionManager)
            .AddPolicy(nameof(Policies.LoggedInUser), Policies.LoggedInUser)
            .AddPolicy(nameof(Policies.SiteMaintainer), Policies.SiteMaintainer)
            .AddPolicy(nameof(Policies.StaffUser), Policies.StaffUser)
            .AddPolicy(nameof(Policies.UserAdministrator), Policies.UserAdministrator);

        services.AddSingleton<IAuthorizationHandler, ComplaintViewPermissionsHandler>();
        services.AddSingleton<IAuthorizationHandler, ComplaintUpdatePermissionsHandler>();
    }
}
