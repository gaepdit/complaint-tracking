using Microsoft.AspNetCore.Authorization;

namespace Cts.AppServices.Permissions.Requirements;

internal class ActiveUserRequirement :
    AuthorizationHandler<ActiveUserRequirement>, IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ActiveUserRequirement requirement)
    {
        if (context.User.HasClaim(c => c.Type == nameof(Policies.ActiveUser) && c.Value == true.ToString()))
            context.Succeed(requirement);

        return Task.FromResult(0);
    }
}
