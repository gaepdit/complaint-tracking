using Cts.AppServices.Complaints.CommandDto;
using Cts.AppServices.Permissions.Helpers;
using Cts.Domain;
using Cts.Domain.Entities.Complaints;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Cts.AppServices.Permissions.Requirements;

public class ComplaintUpdateRequirement :
    AuthorizationHandler<ComplaintUpdateRequirement, ComplaintUpdateDto>, IAuthorizationRequirement
{
    private ClaimsPrincipal _user = default!;
    private ComplaintUpdateDto _resource = default!;

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ComplaintUpdateRequirement requirement,
        ComplaintUpdateDto resource)
    {
        _user = context.User;
        _resource = resource;

        if (IsOpen() && UserHasEditAccess() && !UserMustAccept())
            context.Succeed(requirement);

        return Task.FromResult(0);
    }

    private bool UserHasEditAccess() => IsCurrentOwner() || IsCurrentManager() || IsRecentReporter();
    private bool UserMustAccept() => IsCurrentOwner() && IsNotAccepted() && NoReviewPending();

    // Resource properties
    private bool IsOpen() => _resource is { ComplaintClosed: false, IsDeleted: false };
    private bool IsNotAccepted() => _resource is { CurrentOwnerAcceptedDate: null };
    private bool NoReviewPending() => _resource is not { Status: ComplaintStatus.ReviewPending };

    // User status
    private bool IsCurrentOwner() => _resource.CurrentOwnerId == _user.GetUserIdValue();

    private bool IsCurrentManager() =>
        _user.IsManager() && _resource.CurrentOfficeId == _resource.CurrentUserOfficeId ||
        _user.IsDivisionManager();

    private bool IsRecentReporter() =>
        _resource.EnteredById == _user.GetUserIdValue() &&
        _resource.EnteredDate.AddHours(AppConstants.RecentReporterDuration) > DateTimeOffset.Now;
}
