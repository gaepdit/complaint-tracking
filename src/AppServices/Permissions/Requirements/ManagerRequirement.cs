using Microsoft.AspNetCore.Authorization;
using Cts.AppServices.Permissions.Helpers;

namespace Cts.AppServices.Permissions.Requirements;

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
