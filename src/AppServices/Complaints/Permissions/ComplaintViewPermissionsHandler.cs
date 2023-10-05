using Cts.AppServices.Complaints.Dto;
using Cts.Domain;
using Cts.Domain.Entities.Complaints;
using Microsoft.AspNetCore.Authorization;
using Cts.AppServices.Permissions.Helpers;
using System.Security.Claims;
using System.Security.Principal;

namespace Cts.AppServices.Complaints.Permissions;

// TODO: Review these permissions carefully.
internal class ComplaintViewPermissionsHandler :
    AuthorizationHandler<ComplaintOperation, ComplaintViewDto>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ComplaintOperation requirement,
        ComplaintViewDto resource)
    {
        if (!(context.User.Identity?.IsAuthenticated ?? false))
            return Task.FromResult(0);

        var success = requirement.Name switch
        {
            nameof(ComplaintOperation.ManageDeletions) =>
                // Only the Division Manager can delete or restore.
                context.User.IsDivisionManager(),

            nameof(ComplaintOperation.ViewAsOwner) =>
                IsCurrentOwner(context.User, resource),

            nameof(ComplaintOperation.Accept) =>
                NoReviewPending(resource) && NotAccepted(resource) && IsCurrentOwner(context.User, resource),

            nameof(ComplaintOperation.Edit) =>
                IsOpen(resource) && IsCurrentOwnerOrManager(context.User, resource),

            nameof(ComplaintOperation.EditAsRecentReporter) =>
                IsRecentReporter(context.User, resource),

            nameof(ComplaintOperation.Review) =>
                ReviewPending(resource) && IsCurrentManager(context.User, resource),

            nameof(ComplaintOperation.Assign) =>
                NoReviewPending(resource) && NoCurrentOwner(resource) &&
                IsCurrentManagerOrAssignor(context.User, resource),

            nameof(ComplaintOperation.RequestReview) =>
                NoReviewPending(resource) && IsCurrentOwnerOrManager(context.User, resource),

            nameof(ComplaintOperation.Reassign) =>
                NoReviewPending(resource) && IsCurrentOwnerOrManager(context.User, resource) &&
                HasCurrentOwner(resource),

            nameof(ComplaintOperation.Reopen) =>
                // Only the Division Manager can reopen.
                IsClosed(resource) && context.User.IsDivisionManager(),

            nameof(ComplaintOperation.EditAttachments) =>
                context.User.IsAttachmentsEditor() || IsOpen(resource) &&
                (IsCurrentOwnerOrManager(context.User, resource) || IsRecentReporter(context.User, resource)),

            _ => throw new ArgumentOutOfRangeException(nameof(requirement)),
        };

        if (success) context.Succeed(requirement);
        return Task.FromResult(0);
    }

    private static bool NotAccepted(ComplaintViewDto resource) =>
        resource is { CurrentOwnerAcceptedDate: null };

    private static bool IsOpen(ComplaintViewDto resource) =>
        resource is { ComplaintClosed: false, IsDeleted: false };

    private static bool IsClosed(ComplaintViewDto resource) =>
        resource is { ComplaintClosed: true, IsDeleted: false };

    private static bool NoReviewPending(ComplaintViewDto resource) =>
        resource is { Status: not ComplaintStatus.ReviewPending, ComplaintClosed: false, IsDeleted: false };

    private static bool ReviewPending(ComplaintViewDto resource) =>
        resource is { Status: ComplaintStatus.ReviewPending, ComplaintClosed: false, IsDeleted: false };

    private static bool NoCurrentOwner(ComplaintViewDto resource) =>
        resource is { CurrentOwner: null };

    private static bool HasCurrentOwner(ComplaintViewDto resource) =>
        resource is { CurrentOwner: not null };

    // Managers can edit within their office.
    private static bool IsCurrentManager(IPrincipal user, ComplaintViewDto resource) =>
        user.IsManager() && resource.CurrentOffice?.Id == resource.CurrentUserOfficeId;

    // Users can edit their own.
    private static bool IsCurrentOwner(ClaimsPrincipal user, ComplaintViewDto resource) =>
        user.IsStaff() && resource.CurrentOwner?.Id == user.GetUserIdValue();

    private static bool IsCurrentOwnerOrManager(ClaimsPrincipal user, ComplaintViewDto resource) =>
        IsCurrentOwner(user, resource) || IsCurrentManager(user, resource);

    // Assignors can reassign within their office.
    private static bool IsCurrentAssignor(ClaimsPrincipal user, ComplaintViewDto resource) =>
        resource.CurrentOffice?.Assignor?.Id == user.GetUserIdValue();

    private static bool IsCurrentManagerOrAssignor(ClaimsPrincipal user, ComplaintViewDto resource) =>
        IsCurrentManager(user, resource) || IsCurrentAssignor(user, resource);

    // Original reporter (or current manager) can edit for a limited duration.
    private static bool IsRecentReporter(ClaimsPrincipal user, ComplaintViewDto resource) =>
        (user.IsStaff() || IsCurrentManager(user, resource)) &&
        resource.EnteredBy?.Id == user.GetUserIdValue() &&
        resource.EnteredDate.AddHours(AppConstants.RecentReporterDuration) > DateTimeOffset.Now;
}
