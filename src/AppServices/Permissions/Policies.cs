using Cts.AppServices.Permissions.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace Cts.AppServices.Permissions;

#pragma warning disable S125 // Sections of code should not be commented out
//
// Two ways to use these policies:
//
// A. As an attribute on a PageModel class (must be registered first in `AddAuthorizationPolicies`):
//
//    [Authorize(Policy = nameof(Policies.ActiveUser))]
//    public class AddModel : PageModel
//
// B. From a DI authorization service: 
//
//    public async Task<IActionResult> OnGetAsync([FromServices] IAuthorizationService authorization)
//    {
//        var isStaff = (await authorization.AuthorizeAsync(User, Policies.StaffUser)).Succeeded;
//
//        // or, with `using AuthorizationServiceExtensions;`:
//        var isStaff =  await authorization.Succeeded(User, Policies.StaffUser);
//    }
//
#pragma warning restore S125

public static class Policies
{
    // Default policy builder
    private static AuthorizationPolicyBuilder ActiveUserPolicyBuilder => new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser().AddRequirements(new ActiveUserRequirement());

    // Claims-based policies
    public static AuthorizationPolicy ActiveUser { get; } =
        ActiveUserPolicyBuilder.Build();

    // Role-based policies
    public static AuthorizationPolicy DataExporter { get; } =
        ActiveUserPolicyBuilder.AddRequirements(new DataExporterRequirement()).Build();

    public static AuthorizationPolicy DivisionManager { get; } =
        ActiveUserPolicyBuilder.AddRequirements(new DivisionManagerRequirement()).Build();

    public static AuthorizationPolicy Manager { get; } =
        ActiveUserPolicyBuilder.AddRequirements(new ManagerRequirement()).Build();

    public static AuthorizationPolicy SiteMaintainer { get; } =
        ActiveUserPolicyBuilder.AddRequirements(new SiteMaintainerRequirement()).Build();

    public static AuthorizationPolicy StaffUser { get; } =
        ActiveUserPolicyBuilder.AddRequirements(new StaffUserRequirement()).Build();

    public static AuthorizationPolicy UserAdministrator { get; } =
        ActiveUserPolicyBuilder.AddRequirements(new UserAdminRequirement()).Build();

    public static AuthorizationPolicy SuperUserAdministrator { get; } =
        ActiveUserPolicyBuilder.AddRequirements(new SuperUserAdminRequirement()).Build();
}
