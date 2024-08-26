using Cts.AppServices.Complaints.Permissions;
using Cts.AppServices.Permissions;
using Cts.AppServices.Permissions.AppClaims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Cts.AppServices.RegisterServices;

[SuppressMessage("Major Code Smell", "S125:Sections of code should not be commented out")]
public static class AuthorizationPolicies
{
    public static void AddAuthorizationPolicies(this IServiceCollection services)
    {
        // These policies are for use in PageModel class attributes, e.g.:
        // [Authorize(Policy = nameof(Policies.ActiveUser))]

        services.AddAuthorizationBuilder()
            .AddPolicy(nameof(Policies.ActiveUser), Policies.ActiveUser)
            .AddPolicy(nameof(Policies.DataExporter), Policies.DataExporter)
            .AddPolicy(nameof(Policies.DivisionManager), Policies.DivisionManager)
            .AddPolicy(nameof(Policies.Manager), Policies.Manager)
            .AddPolicy(nameof(Policies.SiteMaintainer), Policies.SiteMaintainer)
            .AddPolicy(nameof(Policies.StaffUser), Policies.StaffUser)
            .AddPolicy(nameof(Policies.SuperUserAdministrator), Policies.SuperUserAdministrator)
            .AddPolicy(nameof(Policies.UserAdministrator), Policies.UserAdministrator);

        // Resource/operation-based permission handlers, e.g.:
        // var canAssign = await authorization.Succeeded(User, complaintView, ComplaintOperation.Assign);

        services.AddSingleton<IAuthorizationHandler, ComplaintViewRequirement>();

        // Add claims transformations
        services.AddScoped<IClaimsTransformation, AppClaimsTransformation>();
    }
}
