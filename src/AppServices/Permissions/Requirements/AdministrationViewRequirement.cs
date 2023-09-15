using Microsoft.AspNetCore.Authorization;
using Cts.Domain.Identity;

namespace Cts.AppServices.Permissions.Requirements;

internal class AdministrationViewRequirement :
    AuthorizationHandler<AdministrationViewRequirement>, IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        AdministrationViewRequirement requirement)
    {
        if (context.User.IsInRole(RoleName.Staff) ||
            context.User.IsInRole(RoleName.SiteMaintenance) ||
            context.User.IsInRole(RoleName.DivisionManager))
            context.Succeed(requirement);

        return Task.FromResult(0);
    }
}
