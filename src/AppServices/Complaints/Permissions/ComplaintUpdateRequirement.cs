using Cts.AppServices.Complaints.CommandDto;
using Cts.AppServices.Permissions.AppClaims;
using Cts.AppServices.Permissions.Helpers;
using Cts.Domain;
using Cts.Domain.Entities.Complaints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using System.Security.Claims;

namespace Cts.AppServices.Complaints.Permissions;

public class ComplaintUpdateRequirement :
    AuthorizationHandler<ComplaintUpdateRequirement, ComplaintUpdateDto>, IAuthorizationRequirement
{
    private ClaimsPrincipal _user = null!;
    private ComplaintUpdateDto _resource = null!;

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
    private bool IsCurrentOwner() => _resource.CurrentOwnerId == _user.GetNameIdentifierId();

    private bool IsCurrentManager() =>
        _user.IsManager() &&
        _user.HasRealClaim(AppClaimTypes.OfficeId, _resource.CurrentOfficeId?.ToString()) ||
        _user.IsDivisionManager();

    private bool IsRecentReporter() =>
        _resource.EnteredById == _user.GetNameIdentifierId() &&
        _resource.EnteredDate.AddHours(AppConstants.RecentReporterDuration) > DateTimeOffset.Now;
}
