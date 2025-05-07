using Cts.AppServices.IdentityServices.Roles;
using Microsoft.AspNetCore.Authorization;

namespace Cts.AppServices.AuthorizationPolicies.Requirements;

internal class DivisionManagerRequirement :
    AuthorizationHandler<DivisionManagerRequirement>, IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        DivisionManagerRequirement requirement)
    {
        if (context.User.IsDivisionManager())
            context.Succeed(requirement);

        return Task.FromResult(0);
    }
}
