using Cts.AppServices.Permissions.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace Cts.AppServices.Permissions;

#pragma warning disable S125 // Sections of code should not be commented out
//
// Two ways to use these policies:
//
// A. As an attribute on a PageModel class:
//
//    [Authorize(Policy = nameof(Policies.SiteMaintainer))]
//    public class AddModel : PageModel
//
// B. From a DI authorization service: 
//
//    public async Task<IActionResult> OnGetAsync([FromServices] IAuthorizationService authorizationService)
//    {
//        var isStaff = (await authorizationService.AuthorizeAsync(User, Policies.StaffUser)).Succeeded;
//    }
//
#pragma warning restore S125

public static class Policies
{
    // Default policy builders
    private static AuthorizationPolicyBuilder AuthenticatedUserPolicyBuilder =>
        new AuthorizationPolicyBuilder().RequireAuthenticatedUser();

    private static AuthorizationPolicyBuilder ActiveUserPolicyBuilder =>
        AuthenticatedUserPolicyBuilder.AddRequirements(new ActiveUserRequirement());

    // Basic policies
    public static AuthorizationPolicy ActiveUser => ActiveUserPolicyBuilder.Build();

    public static AuthorizationPolicy LoggedInUser => AuthenticatedUserPolicyBuilder.Build();

    // Role-based policies
    public static AuthorizationPolicy AttachmentsEditor =>
        ActiveUserPolicyBuilder.AddRequirements(new AttachmentsEditorRequirement()).Build();

    public static AuthorizationPolicy DataExporter =>
        ActiveUserPolicyBuilder.AddRequirements(new DataExporterRequirement()).Build();

    public static AuthorizationPolicy DivisionManager =>
        ActiveUserPolicyBuilder.AddRequirements(new DivisionManagerRequirement()).Build();

    public static AuthorizationPolicy Manager =>
        ActiveUserPolicyBuilder.AddRequirements(new ManagerRequirement()).Build();

    public static AuthorizationPolicy SiteMaintainer =>
        ActiveUserPolicyBuilder.AddRequirements(new SiteMaintainerRequirement()).Build();

    public static AuthorizationPolicy StaffUser =>
        ActiveUserPolicyBuilder.AddRequirements(new StaffUserRequirement()).Build();

    public static AuthorizationPolicy UserAdministrator =>
        ActiveUserPolicyBuilder.AddRequirements(new UserAdminRequirement()).Build();
}
