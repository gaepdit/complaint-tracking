using Cts.AppServices.Offices;
using Cts.AppServices.Permissions.Helpers;
using Cts.Domain.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Cts.AppServices.Permissions.Requirements;

public class OfficeAssignmentRequirement :
    AuthorizationHandler<OfficeAssignmentRequirement, OfficeAndUser>, IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        OfficeAssignmentRequirement requirement,
        OfficeAndUser resource)
    {
        if (UserCanAssignForOffice(resource.Office, resource.User) || context.User.IsDivisionManager())
            context.Succeed(requirement);

        return Task.FromResult(0);
    }

    private static bool UserCanAssignForOffice(OfficeWithAssignorDto? office, ApplicationUser? user) =>
        user != null && office is { Active: true } &&
        (office.Assignor?.Id == user.Id || office.Id == user.Office?.Id);
}

public record OfficeAndUser(OfficeWithAssignorDto? Office, ApplicationUser? User);
