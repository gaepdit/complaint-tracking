using Microsoft.AspNetCore.Authorization;

namespace Cts.Domain.Security.Policies;

internal class SiteMaintainerRequirement : AuthorizationHandler<SiteMaintainerRequirement>, IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        SiteMaintainerRequirement requirement)
    {
        if (context.User.IsInRole(RoleName.SiteMaintenance) ||
            context.User.IsInRole(RoleName.DivisionManager))
        {
            context.Succeed(requirement);
        }

        return Task.FromResult(0);
    }
}
