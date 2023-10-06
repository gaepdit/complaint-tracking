using Cts.AppServices.Complaints.Dto;
using Cts.AppServices.Permissions.Helpers;
using Cts.Domain;
using Cts.Domain.Entities.Complaints;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Cts.AppServices.Complaints.Permissions;

// TODO: Review these permissions carefully.
internal class ComplaintViewPermissionsHandler :
    AuthorizationHandler<ComplaintOperation, ComplaintViewDto>
{
    private ClaimsPrincipal _user = default!;
    private ComplaintViewDto _resource = default!;

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ComplaintOperation requirement,
        ComplaintViewDto resource)
    {
        _user = context.User;
        if (!(_user.Identity?.IsAuthenticated ?? false))
            return Task.FromResult(0);
        _resource = resource;

        var success = requirement.Name switch
        {
            nameof(ComplaintOperation.ManageDeletions) =>
                _user.IsDivisionManager(), // Only the Division Manager can delete or restore.

            nameof(ComplaintOperation.ViewAsOwner) =>
                IsCurrentOwner(),

            nameof(ComplaintOperation.Accept) =>
                IsOpen() && !ReviewPending() && !Accepted() && IsCurrentOwner(),

            nameof(ComplaintOperation.EditDetails) =>
                IsOpen() && !MustAccept() && UserHasEditAccess(),

            nameof(ComplaintOperation.EditActions) =>
                IsOpen() && !MustAccept() && IsCurrentOwnerOrManager(),

            nameof(ComplaintOperation.Review) =>
                IsOpen() && ReviewPending() && IsCurrentManager(),

            nameof(ComplaintOperation.Assign) =>
                !ReviewPending() && NoCurrentOwner() && IsCurrentManagerOrAssignor(),

            nameof(ComplaintOperation.RequestReview) =>
                IsOpen() && !ReviewPending() && IsCurrentOwnerOrManager(),

            nameof(ComplaintOperation.Reassign) =>
                IsOpen() && !ReviewPending() && IsCurrentOwnerOrManager() && HasCurrentOwner(),

            nameof(ComplaintOperation.Reopen) =>
                IsClosed() && _user.IsDivisionManager(), // Only the Division Manager can reopen.

            nameof(ComplaintOperation.EditAttachments) =>
                _user.IsAttachmentsEditor() || // Attachments editor can edit attachments on closed complaints.
                IsOpen() && (IsCurrentOwnerOrManager() || IsRecentReporter()),

            _ => throw new ArgumentOutOfRangeException(nameof(requirement)),
        };

        if (success) context.Succeed(requirement);
        return Task.FromResult(0);
    }

    private bool UserHasEditAccess() => _user.IsDivisionManager() || IsCurrentOwnerOrManager() || IsRecentReporter();

    private bool MustAccept() => IsCurrentOwner() && !Accepted() && !ReviewPending();

    private bool Accepted() => _resource is not { CurrentOwnerAcceptedDate: null };

    private bool IsOpen() => _resource is { ComplaintClosed: false, IsDeleted: false };

    private bool IsClosed() => _resource is { ComplaintClosed: true, IsDeleted: false };

    private bool ReviewPending() => _resource is { Status: ComplaintStatus.ReviewPending };

    private bool NoCurrentOwner() => _resource is { CurrentOwner: null };

    private bool HasCurrentOwner() => _resource is { CurrentOwner: not null };

    // Managers can edit within their office.
    private bool IsCurrentManager() =>
        _user.IsManager() && _resource.CurrentOffice?.Id == _resource.CurrentUserOfficeId;

    // Users can edit their own.
    private bool IsCurrentOwner() => _user.IsStaff() && _resource.CurrentOwner?.Id == _user.GetUserIdValue();

    private bool IsCurrentOwnerOrManager() => IsCurrentOwner() || IsCurrentManager();

    // Assignors can reassign within their office.
    private bool IsCurrentAssignor() => _resource.CurrentOffice?.Assignor?.Id == _user.GetUserIdValue();

    private bool IsCurrentManagerOrAssignor() => IsCurrentManager() || IsCurrentAssignor();

    // Original reporter can edit for a limited duration.
    private bool IsRecentReporter() =>
        _resource.EnteredBy?.Id == _user.GetUserIdValue() &&
        _resource.EnteredDate.AddHours(AppConstants.RecentReporterDuration) > DateTimeOffset.Now;
}
