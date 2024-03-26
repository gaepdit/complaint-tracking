using Cts.AppServices.Complaints.CommandDto;
using Cts.AppServices.Complaints.Permissions;
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

        if (IsOpen() && !MustAccept() && UserCanEdit())
            context.Succeed(requirement);

        return Task.FromResult(0);
    }

    private bool IsOpen() => _resource is { ComplaintClosed: false, IsDeleted: false };
    private bool MustAccept() => IsCurrentOwner() && !Accepted() && !ReviewPending();
    private bool Accepted() => _resource is not { CurrentOwnerAcceptedDate: null };
    private bool ReviewPending() => _resource is { Status: ComplaintStatus.ReviewPending };
    private bool IsCurrentOwner() => _resource.CurrentOwnerId == _user.GetUserIdValue();
    private bool UserCanEdit() => _user.IsDivisionManager() || IsCurrentOwnerOrManager() || IsRecentReporter();
    private bool IsCurrentOwnerOrManager() => IsCurrentOwner() || IsCurrentManager();
    private bool IsCurrentManager() => _user.IsManager() && _resource.CurrentOfficeId == _resource.CurrentUserOfficeId;

    private bool IsRecentReporter() =>
        _resource.EnteredById == _user.GetUserIdValue() &&
        _resource.EnteredDate.AddHours(AppConstants.RecentReporterDuration) > DateTimeOffset.Now;
}
