using Cts.Domain.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Cts.AppServices.Permissions.Requirements;

internal class DivisionManagerRequirement :
    AuthorizationHandler<DivisionManagerRequirement>, IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        DivisionManagerRequirement requirement)
    {
        if (context.User.IsInRole(RoleName.DivisionManager))
            context.Succeed(requirement);

        return Task.FromResult(0);
    }
}
