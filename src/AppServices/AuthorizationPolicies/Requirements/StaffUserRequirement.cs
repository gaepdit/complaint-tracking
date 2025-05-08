using Cts.AppServices.AuthenticationServices.Roles;
using Microsoft.AspNetCore.Authorization;

namespace Cts.AppServices.AuthorizationPolicies.Requirements;

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
