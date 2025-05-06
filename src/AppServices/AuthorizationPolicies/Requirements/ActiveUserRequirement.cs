using Cts.AppServices.AuthenticationServices.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Cts.AppServices.AuthorizationPolicies.Requirements;

internal class ActiveUserRequirement :
    AuthorizationHandler<ActiveUserRequirement>, IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ActiveUserRequirement requirement)
    {
        if (context.User.IsActive())
            context.Succeed(requirement);

        return Task.FromResult(0);
    }
}
