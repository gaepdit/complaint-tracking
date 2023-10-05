using Microsoft.AspNetCore.Authorization;
using Cts.AppServices.Permissions.Helpers;

namespace Cts.AppServices.Permissions.Requirements;

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
