using Cts.AppServices.AuthenticationServices.Roles;
using Microsoft.AspNetCore.Authorization;

namespace Cts.AppServices.AuthorizationPolicies.Requirements;

internal class SiteMaintainerRequirement :
    AuthorizationHandler<SiteMaintainerRequirement>, IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        SiteMaintainerRequirement requirement)
    {
        if (context.User.IsSiteMaintainer())
            context.Succeed(requirement);

        return Task.FromResult(0);
    }
}
