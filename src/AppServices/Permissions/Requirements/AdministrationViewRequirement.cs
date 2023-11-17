using Microsoft.AspNetCore.Authorization;
using Cts.AppServices.Permissions.Helpers;

namespace Cts.AppServices.Permissions.Requirements;

internal class AdministrationViewRequirement 
    : AuthorizationHandler<AdministrationViewRequirement>, IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        AdministrationViewRequirement requirement)
    {
        if (context.User.IsStaffOrMaintainer())
            context.Succeed(requirement);

        return Task.FromResult(0);
    }
}
