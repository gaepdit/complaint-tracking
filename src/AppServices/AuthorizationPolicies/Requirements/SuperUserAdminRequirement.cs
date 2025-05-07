using Cts.AppServices.IdentityServices.Roles;
using Microsoft.AspNetCore.Authorization;

namespace Cts.AppServices.AuthorizationPolicies.Requirements;

internal class SuperUserAdminRequirement :
    AuthorizationHandler<SuperUserAdminRequirement>, IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        SuperUserAdminRequirement requirement)
    {
        if (context.User.IsSuperUserAdmin())
            context.Succeed(requirement);

        return Task.FromResult(0);
    }
}
