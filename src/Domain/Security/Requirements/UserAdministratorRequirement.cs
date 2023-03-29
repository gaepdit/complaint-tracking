using Microsoft.AspNetCore.Authorization;

namespace Cts.Domain.Security.Policies;

internal class UserAdministratorRequirement : AuthorizationHandler<UserAdministratorRequirement>, IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        UserAdministratorRequirement requirement)
    {
        if (context.User.IsInRole(RoleName.UserAdmin) ||
            context.User.IsInRole(RoleName.DivisionManager))
        {
            context.Succeed(requirement);
        }

        return Task.FromResult(0);
    }
}
