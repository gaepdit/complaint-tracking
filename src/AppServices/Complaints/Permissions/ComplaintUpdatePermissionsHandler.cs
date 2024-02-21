using Cts.AppServices.Complaints.CommandDto;
using Cts.AppServices.Permissions.Helpers;
using Cts.Domain;
using Cts.Domain.Entities.Complaints;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Cts.AppServices.Complaints.Permissions;

internal class ComplaintUpdatePermissionsHandler :
    AuthorizationHandler<ComplaintOperation, ComplaintUpdateDto>
{
    private ClaimsPrincipal _user = default!;
    private ComplaintUpdateDto _resource = default!;

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ComplaintOperation requirement,
        ComplaintUpdateDto resource)
    {
        _user = context.User;
        if (!(_user.Identity?.IsAuthenticated ?? false))
            return Task.FromResult(0);
        _resource = resource;

        if (requirement.Name == nameof(ComplaintOperation.EditDetails) &&
            IsOpen() && !MustAccept() && UserHasEditAccess())
        {
            context.Succeed(requirement);
        }

        return Task.FromResult(0);
    }

    private bool IsOpen() => _resource is { ComplaintClosed: false, IsDeleted: false };

    private bool MustAccept() => IsCurrentOwner() && !Accepted() && !ReviewPending();

    // Users can edit their own.
    private bool IsCurrentOwner() =>
        _user.IsStaff() && _resource.CurrentOwnerId == _user.GetUserIdValue();

    private bool Accepted() => _resource is not { CurrentOwnerAcceptedDate: null };
    private bool ReviewPending() => _resource is { Status: ComplaintStatus.ReviewPending };

    private bool UserHasEditAccess() => _user.IsDivisionManager() || IsCurrentOwnerOrManager() || IsRecentReporter();

    private bool IsCurrentOwnerOrManager() => IsCurrentOwner() || IsCurrentManager();

    // Managers can edit within their office.
    private bool IsCurrentManager() =>
        _user.IsManager() && _resource.CurrentOfficeId == _resource.CurrentUserOfficeId;

    // Original reporter can edit for a limited duration.
    private bool IsRecentReporter() =>
        _resource.EnteredById == _user.GetUserIdValue() &&
        _resource.EnteredDate.AddHours(AppConstants.RecentReporterDuration) > DateTimeOffset.Now;
}
