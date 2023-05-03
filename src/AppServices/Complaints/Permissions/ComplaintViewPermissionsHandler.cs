using Cts.AppServices.Complaints.Dto;
using Cts.Domain.Entities.Complaints;
using Cts.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Security.Principal;

namespace Cts.AppServices.Complaints.Permissions;

internal class ComplaintViewPermissionsHandler :
    AuthorizationHandler<ComplaintOperation, ComplaintViewDto>
{
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

    // Division Managers can edit all.
    private static bool IsDivisionManager(IPrincipal user) =>
        user.IsInRole(RoleName.DivisionManager);

    // Managers can edit within their office.
    private static bool IsCurrentManager(IPrincipal user, ComplaintViewDto resource) =>
        user.IsInRole(RoleName.DivisionManager)
        || user.IsInRole(RoleName.Manager) && resource.CurrentOffice?.Id == resource.CurrentUserOfficeId;

    // Users can edit their own.
    private static bool IsCurrentOwner(ClaimsPrincipal user, ComplaintViewDto resource) =>
        user.IsInRole(RoleName.Staff) && resource.CurrentOwner?.Id == user.GetUserIdValue();

    private static bool IsCurrentOwnerOrManager(ClaimsPrincipal user, ComplaintViewDto resource) =>
        IsCurrentOwner(user, resource) || IsCurrentManager(user, resource);

    // Assignors can reassign within their office.
    private static bool IsCurrentAssignor(ClaimsPrincipal user, ComplaintViewDto resource) =>
        NoCurrentOwner(resource) &&
        resource.CurrentOffice?.Assignor?.Id == user.GetUserIdValue();

    private static bool IsCurrentManagerOrAssignor(ClaimsPrincipal user, ComplaintViewDto resource) =>
        IsCurrentManager(user, resource) || IsCurrentAssignor(user, resource);

    // Original reporter can edit for 1 hour.
    private static bool IsRecentReporter(ClaimsPrincipal user, ComplaintViewDto resource) =>
        (user.IsInRole(RoleName.Staff) || IsCurrentManager(user, resource)) &&
        resource.EnteredBy?.Id == user.GetUserIdValue() &&
        resource.EnteredDate.AddHours(1) > DateTimeOffset.Now;

    // Attachment editors and Division Managers can always edit attachments.
    private static bool IsAlwaysAttachmentEditor(IPrincipal user, ComplaintViewDto resource) =>
        resource is { IsDeleted: false } &&
        (IsDivisionManager(user) || user.IsInRole(RoleName.AttachmentsEditor));

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ComplaintOperation requirement,
        ComplaintViewDto resource)
    {
        if (!(context.User.Identity?.IsAuthenticated ?? false))
            return Task.FromResult(0);

        var success = requirement.Name switch
        {
            ComplaintOperationNames.ManageDeletions =>
                // Only the Division Manager can delete or restore.
                IsDivisionManager(context.User),

            ComplaintOperationNames.ViewAsOwner =>
                IsCurrentOwner(context.User, resource),

            ComplaintOperationNames.Accept =>
                NoReviewPending(resource) &&
                IsCurrentOwner(context.User, resource) &&
                resource.CurrentOwnerAcceptedDate == null,

            ComplaintOperationNames.Edit =>
                IsOpen(resource) && IsCurrentOwnerOrManager(context.User, resource),

            ComplaintOperationNames.EditAsRecentReporter =>
                IsRecentReporter(context.User, resource),

            ComplaintOperationNames.Review =>
                ReviewPending(resource) && IsCurrentManager(context.User, resource),

            ComplaintOperationNames.Assign =>
                NoReviewPending(resource) && NoCurrentOwner(resource) &&
                IsCurrentManagerOrAssignor(context.User, resource),

            ComplaintOperationNames.RequestReview =>
                NoReviewPending(resource) && IsCurrentOwnerOrManager(context.User, resource),

            ComplaintOperationNames.Reassign =>
                NoReviewPending(resource) && IsCurrentOwnerOrManager(context.User, resource) &&
                HasCurrentOwner(resource),

            ComplaintOperationNames.Reopen =>
                IsClosed(resource) && IsDivisionManager(context.User),

            ComplaintOperationNames.EditAttachments =>
                IsAlwaysAttachmentEditor(context.User, resource) ||
                IsOpen(resource) &&
                (IsCurrentOwnerOrManager(context.User, resource) || IsRecentReporter(context.User, resource)),

            _ => throw new ArgumentOutOfRangeException(nameof(requirement)),
        };

        if (success) context.Succeed(requirement);
        return Task.FromResult(0);
    }
}

public static class ClaimsPrincipalExtensions
{
    public static string? GetUserIdValue(this ClaimsPrincipal principal) =>
        principal.FindFirstValue(ClaimTypes.NameIdentifier);
}
