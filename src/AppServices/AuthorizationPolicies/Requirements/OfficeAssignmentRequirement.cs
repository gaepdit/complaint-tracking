using Cts.AppServices.AuthenticationServices.Claims;
using Cts.AppServices.AuthenticationServices.Roles;
using Cts.AppServices.Offices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using System.Security.Claims;

namespace Cts.AppServices.AuthorizationPolicies.Requirements;

public class OfficeAssignmentRequirement :
    AuthorizationHandler<OfficeAssignmentRequirement, OfficeWithAssignorDto>, IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        OfficeAssignmentRequirement requirement,
        OfficeWithAssignorDto resource)
    {
        if (resource.Active && UserCanAssignForOffice(resource, context.User))
            context.Succeed(requirement);

        return Task.FromResult(0);
    }

    private static bool UserCanAssignForOffice(OfficeWithAssignorDto resource, ClaimsPrincipal user) =>
        resource.Assignor?.Id == user.GetNameIdentifierId() ||
        user.HasClaim(AppClaimTypes.OfficeId, resource.Id.ToString()) ||
        user.IsDivisionManager();
}
