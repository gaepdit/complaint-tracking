using Cts.AppServices.Permissions.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace Cts.AppServices.Permissions;

#pragma warning disable S125
//
// Two ways to use these policies:
//
// A. As an attribute on a PageModel class:
//
//    [Authorize(Policy = PolicyName.SiteMaintainer)]
//    public class AddModel : PageModel
//
// B. From a DI authorization service: 
//
//    public async Task<IActionResult> OnGetAsync([FromServices] IAuthorizationService authorizationService)
//    {
//        var isStaff = (await authorizationService.AuthorizeAsync(User, Policies.StaffUserPolicy())).Succeeded;
//    }
//
#pragma warning restore S125

public static class PolicyName
{
    public const string StaffUser = nameof(StaffUser);
    public const string SiteMaintainer = nameof(SiteMaintainer);
    public const string UserAdministrator = nameof(UserAdministrator);
    public const string DivisionManager = nameof(DivisionManager);
}

public static class Policies
{
    public static AuthorizationPolicy StaffUserPolicy() =>
        new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .AddRequirements(new StaffUserRequirement())
            .Build();

    public static AuthorizationPolicy SiteMaintainerPolicy() =>
        new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .AddRequirements(new SiteMaintainerRequirement())
            .Build();

    public static AuthorizationPolicy UserAdministratorPolicy() =>
        new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .AddRequirements(new UserAdministratorRequirement())
            .Build();

    public static AuthorizationPolicy DivisionManagerPolicy() =>
        new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .AddRequirements(new DivisionManagerRequirement())
            .Build();
}
