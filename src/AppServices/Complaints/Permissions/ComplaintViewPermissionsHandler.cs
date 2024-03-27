using Cts.AppServices.Complaints.QueryDto;
using Cts.AppServices.Permissions.Helpers;
using Cts.Domain;
using Cts.Domain.Entities.Complaints;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Cts.AppServices.Complaints.Permissions;

internal class ComplaintViewPermissionsHandler : AuthorizationHandler<ComplaintOperation, ComplaintViewDto>
{
    private ClaimsPrincipal _user = default!;
    private ComplaintViewDto _resource = default!;

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ComplaintOperation requirement,
        ComplaintViewDto resource)
    {
        _user = context.User;
        _resource = resource;

        if (!(_user.Identity?.IsAuthenticated ?? false))
            return Task.FromResult(0);

        var success = requirement.Name switch
        {
            nameof(ComplaintOperation.Accept) => UserMustAccept(),
            nameof(ComplaintOperation.Assign) => UserCanAssign(),
            nameof(ComplaintOperation.EditActions) => UserCanEditActions(),
            nameof(ComplaintOperation.EditAttachments) => UserCanEditAttachments(),
            nameof(ComplaintOperation.EditDetails) => UserCanEditDetails(),
            nameof(ComplaintOperation.ManageDeletions) => _user.IsDivisionManager(),
            nameof(ComplaintOperation.Reassign) => UserCanReassign(),
            nameof(ComplaintOperation.Reopen) => UserCanReopen(),
            nameof(ComplaintOperation.RequestReview) => UserCanRequestReview(),
            nameof(ComplaintOperation.Review) => UserCanReview(),
            nameof(ComplaintOperation.ViewAsOwner) => IsCurrentOwner(),
            nameof(ComplaintOperation.ViewDeletedActions) => IsCurrentOwnerOrManager(),
            _ => throw new ArgumentOutOfRangeException(nameof(requirement)),
        };

        if (success) context.Succeed(requirement);
        return Task.FromResult(0);
    }


    // Permissions methods
    private bool UserCanAssign() => IsUnencumbered() && IsNotAssigned() && IsCurrentOwnerOrManagerOrAssignor();

    private bool UserCanEditActions() => IsOpen() && !UserMustAccept() && IsCurrentOwnerOrManager();

    // Attachments editor (and Division Manager) can edit attachments on closed complaints.
    private bool UserCanEditAttachments() => UserCanEditDetails() || (IsNotDeleted() && _user.IsAttachmentsEditor());

    // Users can edit their own complaints, managers can edit within their office, original reporter can edit for a limited duration.
    private bool UserCanEditDetails() => IsOpen() && !UserMustAccept() && IsCurrentOwnerOrManagerOrRecentReporter();

    private bool UserCanReassign() => IsUnencumbered() && IsAssigned() && IsCurrentOwnerOrManager();
    private bool UserCanReopen() => IsClosed() && _user.IsDivisionManager();
    private bool UserCanRequestReview() => IsUnencumbered() && IsCurrentOwnerOrManager();
    private bool UserCanReview() => IsOpen() && !UserMustAccept() && IsReviewPending() && IsCurrentManager();
    private bool UserMustAccept() => IsOpen() && IsNotAccepted() && NoReviewPending() && IsCurrentOwner();
    private bool IsUnencumbered() => IsOpen() && NoReviewPending() && !UserMustAccept();

    // Resource properties
    private bool IsAssigned() => _resource is { CurrentOwner: not null };
    private bool IsNotAssigned() => _resource is { CurrentOwner: null };
    private bool IsClosed() => _resource is { ComplaintClosed: true, IsDeleted: false };
    private bool IsNotAccepted() => _resource is { CurrentOwnerAcceptedDate: null };
    private bool IsNotDeleted() => _resource is { IsDeleted: false };
    private bool IsOpen() => _resource is { ComplaintClosed: false, IsDeleted: false };
    private bool IsReviewPending() => _resource is { Status: ComplaintStatus.ReviewPending };
    private bool NoReviewPending() => _resource is { Status: not ComplaintStatus.ReviewPending };

    // User roles
    private bool IsAssignorForOffice() => _resource.CurrentOffice?.Assignor?.Id == _user.GetUserIdValue();

    private bool IsCurrentManager() =>
        _user.IsManager() && _resource.CurrentOffice?.Id == _resource.CurrentUserOfficeId ||
        _user.IsDivisionManager();

    private bool IsCurrentOwner() => _user.IsStaff() && _resource.CurrentOwner?.Id == _user.GetUserIdValue();

    private bool IsRecentReporter() =>
        _resource.EnteredBy?.Id == _user.GetUserIdValue() &&
        _resource.EnteredDate.AddHours(AppConstants.RecentReporterDuration) > DateTimeOffset.Now;

    // User role combos
    private bool IsCurrentOwnerOrManager() => IsCurrentOwner() || IsCurrentManager();
    private bool IsCurrentOwnerOrManagerOrAssignor() => IsCurrentOwnerOrManager() || IsAssignorForOffice();
    private bool IsCurrentOwnerOrManagerOrRecentReporter() => IsCurrentOwnerOrManager() || IsRecentReporter();
}
