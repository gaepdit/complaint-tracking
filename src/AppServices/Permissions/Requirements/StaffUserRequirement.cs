using Microsoft.AspNetCore.Authorization;
using Cts.AppServices.Permissions.Helpers;

namespace Cts.AppServices.Permissions.Requirements;

internal class StaffUserRequirement :
    AuthorizationHandler<StaffUserRequirement>, IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        StaffUserRequirement requirement)
    {
        if (context.User.IsStaff())
            context.Succeed(requirement);

        return Task.FromResult(0);
    }
}
