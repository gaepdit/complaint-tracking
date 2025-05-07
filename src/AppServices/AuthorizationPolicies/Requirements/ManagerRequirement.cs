using Cts.AppServices.IdentityServices.Roles;
using Microsoft.AspNetCore.Authorization;

namespace Cts.AppServices.AuthorizationPolicies.Requirements;

internal class ManagerRequirement :
    AuthorizationHandler<ManagerRequirement>, IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ManagerRequirement requirement)
    {
        if (context.User.IsManager())
            context.Succeed(requirement);

        return Task.FromResult(0);
    }
}
