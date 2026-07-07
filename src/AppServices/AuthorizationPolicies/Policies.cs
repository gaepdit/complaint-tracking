using Cts.AppServices.AuthenticationServices;
using Cts.AppServices.Complaints.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Cts.AppServices.AuthorizationPolicies;

#pragma warning disable S125 // Sections of code should not be commented out
//
// Ways to use these policies:
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
// C. With resource/operation-based permission handlers:
//
//     var canAssign = await authorization.Succeeded(User, complaintView, ComplaintOperation.Assign);
//
#pragma warning restore S125

public static class Policies
{
    public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
            .AddPolicy(nameof(ActiveUser), ActiveUser)
            .AddPolicy(nameof(DataExporter), DataExporter)
            .AddPolicy(nameof(DivisionManager), DivisionManager)
            .AddPolicy(nameof(Manager), Manager)
            .AddPolicy(nameof(SiteMaintainer), SiteMaintainer)
            .AddPolicy(nameof(StaffUser), StaffUser)
            .AddPolicy(nameof(SuperUserAdministrator), SuperUserAdministrator)
            .AddPolicy(nameof(UserAdministrator), UserAdministrator);

        // Resource/operation-based permission handlers
        services.AddSingleton<IAuthorizationHandler, ComplaintViewRequirement>();

        return services;
    }

    // Default policy builder
    private static AuthorizationPolicyBuilder ActiveUserPolicyBuilder => new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .RequireClaim(AppClaimTypes.ActiveUser, true.ToString());

    // Claims-based policies
    public static AuthorizationPolicy ActiveUser { get; } = ActiveUserPolicyBuilder.Build();

    // Role-based policies
    public static AuthorizationPolicy DataExporter { get; } = ActiveUserPolicyBuilder
        .RequireAssertion(context => context.User.IsDataExporter()).Build();

    public static AuthorizationPolicy DivisionManager { get; } = ActiveUserPolicyBuilder
        .RequireAssertion(context => context.User.IsDivisionManager()).Build();

    public static AuthorizationPolicy Manager { get; } = ActiveUserPolicyBuilder
        .RequireAssertion(context => context.User.IsManager()).Build();

    public static AuthorizationPolicy SiteMaintainer { get; } = ActiveUserPolicyBuilder
        .RequireAssertion(context => context.User.IsSiteMaintainer()).Build();

    public static AuthorizationPolicy StaffUser { get; } = ActiveUserPolicyBuilder
        .RequireAssertion(context => context.User.IsStaff()).Build();

    public static AuthorizationPolicy UserAdministrator { get; } = ActiveUserPolicyBuilder
        .RequireAssertion(context => context.User.IsUserAdmin()).Build();

    public static AuthorizationPolicy SuperUserAdministrator { get; } = ActiveUserPolicyBuilder
        .RequireAssertion(context => context.User.IsSuperUserAdmin()).Build();
}
