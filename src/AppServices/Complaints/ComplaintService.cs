using AutoMapper;
using Cts.AppServices.Attachments;
using Cts.AppServices.Attachments.ValidationAttributes;
using Cts.AppServices.Complaints.CommandDto;
using Cts.AppServices.Complaints.Permissions;
using Cts.AppServices.Complaints.QueryDto;
using Cts.AppServices.Notifications;
using Cts.AppServices.Permissions;
using Cts.AppServices.Permissions.Helpers;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities.Complaints;
using Cts.Domain.Entities.ComplaintTransitions;
using Cts.Domain.Entities.Concerns;
using Cts.Domain.Entities.Offices;
using Cts.Domain.Identity;
using GaEpd.AppLibrary.Extensions;
using GaEpd.AppLibrary.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Cts.AppServices.Complaints;

#pragma warning disable S107
public sealed class ComplaintService(
    IComplaintRepository complaintRepository,
    IComplaintManager complaintManager,
    IConcernRepository concernRepository,
    IOfficeRepository officeRepository,
    IAttachmentService attachmentService,
    INotificationService notificationService,
    // ReSharper disable once SuggestBaseTypeForParameterInConstructor
    IMapper mapper,
    IUserService userService,
    IAuthorizationService authorization,
    ILogger<ComplaintService> logger) : IComplaintService
#pragma warning restore S107
{
    // Public read methods

    public async Task<ComplaintPublicViewDto?> FindPublicAsync(int id, CancellationToken token = default) =>
        mapper.Map<ComplaintPublicViewDto>(await complaintRepository
            .FindPublicAsync(ComplaintFilters.PublicIdPredicate(id), token: token).ConfigureAwait(false));

    public Task<bool> PublicExistsAsync(int id, CancellationToken token = default) =>
        complaintRepository.ExistsAsync(ComplaintFilters.PublicIdPredicate(id), token: token);

    public Task<IPaginatedResult<ComplaintSearchResultDto>> PublicSearchAsync(
        ComplaintPublicSearchDto spec, PaginatedRequest paging, CancellationToken token = default) =>
        PerformPagedSearchAsync(paging, ComplaintFilters.PublicSearchPredicate(spec), token: token);

    // Staff read methods

    public async Task<ComplaintViewDto?> FindAsync(int id, bool includeDeletedActions = false,
        CancellationToken token = default)
    {
        var complaint = await complaintRepository.FindIncludeAllAsync(id, includeDeletedActions, token: token)
            .ConfigureAwait(false);
        if (complaint is null) return null;
        var complaintViewDto = mapper.Map<ComplaintViewDto>(complaint);

        var principal = userService.GetCurrentPrincipal();
        var auth = await authorization.Succeeded(principal!, complaintViewDto, ComplaintOperation.ViewDeletedActions)
            .ConfigureAwait(false);
        if (!auth) complaintViewDto.Actions.RemoveAll(action => action.IsDeleted);

        return complaintViewDto;
    }

    public async Task<ComplaintUpdateDto?> FindForUpdateAsync(int id, CancellationToken token = default) =>
        mapper.Map<ComplaintUpdateDto>(await complaintRepository.FindAsync(e =>
            e.Id == id && !e.IsDeleted && !e.ComplaintClosed, token: token).ConfigureAwait(false));

    public async Task<bool> ExistsAsync(int id, CancellationToken token = default) =>
        await complaintRepository.ExistsAsync(id, token: token).ConfigureAwait(false);

    public async Task<IPaginatedResult<ComplaintSearchResultDto>> SearchAsync(ComplaintSearchDto spec,
        PaginatedRequest paging, CancellationToken token = default)
    {
        var principal = userService.GetCurrentPrincipal();
        if (!await authorization.Succeeded(principal!, Policies.DivisionManager).ConfigureAwait(false))
            spec.DeletedStatus = null;
        return await PerformPagedSearchAsync(paging, ComplaintFilters.SearchPredicate(spec), token: token)
            .ConfigureAwait(false);
    }

    // Private search methods

    private async Task<IPaginatedResult<ComplaintSearchResultDto>> PerformPagedSearchAsync(PaginatedRequest paging,
        Expression<Func<Complaint, bool>> predicate, CancellationToken token)
    {
        var count = await complaintRepository.CountAsync(predicate, token: token).ConfigureAwait(false);
        var items = count > 0
            ? mapper.Map<IEnumerable<ComplaintSearchResultDto>>(await complaintRepository
                .GetPagedListAsync(predicate, paging, token: token).ConfigureAwait(false))
            : [];
        return new PaginatedResult<ComplaintSearchResultDto>(items, count, paging);
    }

    private async Task<IReadOnlyCollection<ComplaintSearchResultDto>> PerformSearchAsync(ComplaintSearchDto spec,
        CancellationToken token) =>
        mapper.Map<IReadOnlyCollection<ComplaintSearchResultDto>>((await complaintRepository
                .GetListAsync(ComplaintFilters.SearchPredicate(spec), token: token).ConfigureAwait(false))
            .OrderByDescending(complaint => complaint.ReceivedDate));

    // Dashboard methods

    public Task<IReadOnlyCollection<ComplaintSearchResultDto>> GetNewComplaintsForUserAsync(string userId,
        CancellationToken token = default)
    {
        var spec = new ComplaintSearchDto { Status = SearchComplaintStatus.NotAccepted, Assigned = userId };
        return PerformSearchAsync(spec, token: token);
    }

    public Task<IReadOnlyCollection<ComplaintSearchResultDto>> GetOpenComplaintsForUserAsync(string userId,
        CancellationToken token = default)
    {
        var spec = new ComplaintSearchDto { Status = SearchComplaintStatus.AllOpen, Assigned = userId };
        return PerformSearchAsync(spec, token: token);
    }

    public Task<IReadOnlyCollection<ComplaintSearchResultDto>> GetReviewPendingComplaintsForUserAsync(string userId,
        CancellationToken token = default)
    {
        var spec = new ComplaintSearchDto { Status = SearchComplaintStatus.ReviewPending, Reviewer = userId };
        return PerformSearchAsync(spec, token: token);
    }

    public Task<IReadOnlyCollection<ComplaintSearchResultDto>> GetUnacceptedComplaintsForOfficeAsync(Guid officeId,
        CancellationToken token = default)
    {
        var spec = new ComplaintSearchDto { Status = SearchComplaintStatus.NotAccepted, Office = officeId };
        return PerformSearchAsync(spec, token: token);
    }

    public Task<IReadOnlyCollection<ComplaintSearchResultDto>> GetUnassignedComplaintsForOfficeAsync(Guid officeId,
        CancellationToken token = default)
    {
        var spec = new ComplaintSearchDto { Status = SearchComplaintStatus.NotAssigned, Office = officeId };
        return PerformSearchAsync(spec, token: token);
    }

    // Staff complaint write methods

    public async Task<ComplaintCreateResult> CreateAsync(ComplaintCreateDto resource,
        IAttachmentService.AttachmentServiceConfig config, string? baseUrl, CancellationToken token = default)
    {
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        var complaint = await CreateComplaintFromDtoAsync(resource, currentUser, token: token).ConfigureAwait(false);

        await complaintRepository.InsertAsync(complaint, autoSave: false, token: token).ConfigureAwait(false);
        await AddTransitionAsync(complaint, TransitionType.New, currentUser, token: token).ConfigureAwait(false);
        await AddAssignmentTransitionsAsync(complaint, currentUser, token: token).ConfigureAwait(false);
        await complaintRepository.SaveChangesAsync(token).ConfigureAwait(false);

        var result = new ComplaintCreateResult(complaint.Id);

        // Send notification
        var template = complaint.CurrentOwner == null
            ? Template.UnassignedComplaint
            : Template.AssignedComplaint;
        var notificationResult =
            await NotifyOwnerAsync(complaint, template, baseUrl, null, token: token).ConfigureAwait(false);
        if (!notificationResult.Success) result.AddWarning(notificationResult.FailureMessage);

        // Process attachments
        if (resource.Files is null || resource.Files.Count <= 0) return result;

        var validateFilesResult = resource.Files.Validate(IAttachmentService.MaxSimultaneousUploads);
        if (validateFilesResult.IsValid)
        {
            result.SetNumberOfAttachments(await attachmentService
                .SaveAttachmentsAsync(complaint.Id, resource.Files, config, token: token)
                .ConfigureAwait(false));
        }
        else
        {
            result.AddWarning("No files were attached: " +
                              validateFilesResult.ValidationErrors.ConcatWithSeparator("; ") + ".");
        }

        return result;
    }

    private async Task<NotificationResult> NotifyOwnerAsync(Complaint complaint, Template template,
        string? baseUrl, string? comment, CancellationToken token)
    {
        var recipient = complaint.CurrentOwner ?? complaint.CurrentOffice.Assignor;

        if (recipient is null)
        {
            logger.LogWarning(NotificationService.NotificationPreparationFailure,
                "Complaint {ComplaintId} notification recipient is null.", complaint.Id);
            return NotificationResult.FailureResult("This complaint does not have an available owner or assignor.");
        }

        if (!recipient.Active)
        {
            logger.LogWarning(NotificationService.NotificationPreparationFailure,
                "Complaint {ComplaintId} notification recipient {RecipientId} is not active in CTS.",
                complaint.Id, recipient.Id);
            return NotificationResult.FailureResult("The complaint owner or assignor is not an active CTS user.");
        }

        if (string.IsNullOrEmpty(recipient.Email))
        {
            logger.LogWarning(NotificationService.NotificationPreparationFailure,
                "Complaint {ComplaintId} notification recipient {RecipientId} email address is null.",
                complaint.Id, recipient.Id);
            return NotificationResult.FailureResult("The complaint owner or assignor cannot be emailed.");
        }

        return await notificationService
            .SendNotificationAsync(template, recipient.Email, complaint, baseUrl, comment, token: token)
            .ConfigureAwait(false);
    }

    public async Task UpdateAsync(int id, ComplaintUpdateDto resource, CancellationToken token = default)
    {
        var complaint = await complaintRepository.GetAsync(id, token: token).ConfigureAwait(false);
        complaint.SetUpdater((await userService.GetCurrentUserAsync().ConfigureAwait(false))?.Id);
        await MapComplaintDetailsAsync(complaint, resource, token: token).ConfigureAwait(false);
        await complaintRepository.UpdateAsync(complaint, token: token).ConfigureAwait(false);
    }

    // Complaint transitions

    public async Task AcceptAsync(int id, CancellationToken token = default)
    {
        var complaint = await complaintRepository.GetAsync(id, token: token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        complaintManager.Accept(complaint, currentUser);
        await complaintRepository.UpdateAsync(complaint, autoSave: false, token: token).ConfigureAwait(false);
        await AddTransitionAsync(complaint, TransitionType.Accepted, currentUser, token: token).ConfigureAwait(false);
        await complaintRepository.SaveChangesAsync(token).ConfigureAwait(false);
    }

    public async Task<ComplaintAssignResult> AssignAsync(ComplaintAssignmentDto resource,
        ComplaintViewDto currentComplaint, string? baseUrl, CancellationToken token = default)
    {
        var result = new ComplaintAssignResult();

        if (resource.OfficeId == currentComplaint.CurrentOffice?.Id &&
            resource.OwnerId == currentComplaint.CurrentOwner?.Id)
            return result;

        var complaint = await complaintRepository.GetAsync(resource.ComplaintId, token: token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        var office = await officeRepository.GetAsync(resource.OfficeId!.Value, Office.IncludeAssignor, token: token)
            .ConfigureAwait(false);
        var owner = resource.OwnerId is not null
            ? await userService.FindUserAsync(resource.OwnerId).ConfigureAwait(false)
            : null;

        complaintManager.Assign(complaint, office, owner, currentUser);
        await complaintRepository.UpdateAsync(complaint, autoSave: false, token: token).ConfigureAwait(false);
        await AddAssignmentTransitionsAsync(complaint, currentUser, token, resource.Comment).ConfigureAwait(false);
        await complaintRepository.SaveChangesAsync(token).ConfigureAwait(false);
        result.IsReassigned = true;

        // Send notification
        var template = complaint.CurrentOwner != null
            ? Template.AssignedComplaint
            : Template.UnassignedComplaint;
        var notificationResult = await NotifyOwnerAsync(complaint, template, baseUrl, null, token: token)
            .ConfigureAwait(false);
        if (!notificationResult.Success) result.AddWarning(notificationResult.FailureMessage);

        return result;
    }

    public async Task<NotificationResult> CloseAsync(ComplaintClosureDto resource, string? baseUrl,
        CancellationToken token = default)
    {
        var complaint = await complaintRepository.GetAsync(resource.ComplaintId, token: token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        complaintManager.Close(complaint, resource.Comment, currentUser);
        await complaintRepository.UpdateAsync(complaint, autoSave: false, token: token).ConfigureAwait(false);
        await AddTransitionAsync(complaint, TransitionType.Closed, currentUser, token, resource.Comment)
            .ConfigureAwait(false);
        await complaintRepository.SaveChangesAsync(token).ConfigureAwait(false);

        // Send notification
        return await NotifyOwnerAsync(complaint, Template.ReviewApproved, baseUrl, resource.Comment, token: token)
            .ConfigureAwait(false);
    }

    public async Task<NotificationResult> ReopenAsync(ComplaintClosureDto resource, string? baseUrl,
        CancellationToken token = default)
    {
        var complaint = await complaintRepository.GetAsync(resource.ComplaintId, token: token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        complaintManager.Reopen(complaint, currentUser);
        await complaintRepository.UpdateAsync(complaint, autoSave: false, token: token).ConfigureAwait(false);
        await AddTransitionAsync(complaint, TransitionType.Reopened, currentUser, token, resource.Comment)
            .ConfigureAwait(false);
        await complaintRepository.SaveChangesAsync(token).ConfigureAwait(false);

        // Send notification
        return await NotifyOwnerAsync(complaint, Template.ComplaintReopened, baseUrl, resource.Comment,
            token).ConfigureAwait(false);
    }

    public async Task<NotificationResult> RequestReviewAsync(ComplaintRequestReviewDto resource, string? baseUrl,
        CancellationToken token = default)
    {
        var complaint = await complaintRepository.GetAsync(resource.ComplaintId, token: token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        var reviewer = (await userService.FindUserAsync(resource.ReviewerId!).ConfigureAwait(false))!;

        complaintManager.RequestReview(complaint, reviewer, currentUser);
        await complaintRepository.UpdateAsync(complaint, autoSave: false, token: token).ConfigureAwait(false);
        await AddTransitionAsync(complaint, TransitionType.SubmittedForReview, currentUser, token, resource.Comment)
            .ConfigureAwait(false);
        await complaintRepository.SaveChangesAsync(token).ConfigureAwait(false);

        // Send notification
        if (!reviewer.Active)
        {
            logger.LogWarning(NotificationService.NotificationPreparationFailure,
                "Complaint {ComplaintId} requested reviewer {RecipientId} is not active in CTS.",
                complaint.Id, reviewer.Id);
            return NotificationResult.FailureResult("The requested reviewer is not an active CTS user.");
        }

        if (string.IsNullOrEmpty(reviewer.Email))
        {
            logger.LogWarning(NotificationService.NotificationPreparationFailure,
                "Complaint {ComplaintId} requested reviewer {RecipientId} email address is null.",
                complaint.Id, reviewer.Id);
            return NotificationResult.FailureResult("The requested reviewer cannot be emailed.");
        }

        return await notificationService.SendNotificationAsync(Template.ReviewRequested, reviewer.Email, complaint,
            baseUrl, resource.Comment, token: token).ConfigureAwait(false);
    }

    public async Task<NotificationResult> ReturnAsync(ComplaintAssignmentDto resource, string? baseUrl,
        CancellationToken token = default)
    {
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        var complaint = await complaintRepository.GetAsync(resource.ComplaintId, token: token).ConfigureAwait(false);
        var previousOwner = complaint.CurrentOwner;
        var newOffice = await officeRepository.GetAsync(resource.OfficeId!.Value, Office.IncludeAssignor, token: token)
            .ConfigureAwait(false);
        var newOwner = resource.OwnerId is not null
            ? await userService.FindUserAsync(resource.OwnerId).ConfigureAwait(false)
            : null;

        complaintManager.Return(complaint, newOffice, newOwner, currentUser);
        await complaintRepository.UpdateAsync(complaint, autoSave: false, token: token).ConfigureAwait(false);
        await AddTransitionAsync(complaint, TransitionType.ReturnedByReviewer, currentUser, token, resource.Comment)
            .ConfigureAwait(false);
        if (complaint.CurrentOwner != null &&
            complaint.CurrentOwner != previousOwner &&
            complaint.CurrentOwner == currentUser)
            await AddTransitionAsync(complaint, TransitionType.Accepted, currentUser, token: token).ConfigureAwait(false);
        await complaintRepository.SaveChangesAsync(token).ConfigureAwait(false);

        // Send notification
        return await NotifyOwnerAsync(complaint, Template.ReviewReturned, baseUrl, resource.Comment, token: token)
            .ConfigureAwait(false);
    }

    public async Task DeleteAsync(ComplaintClosureDto resource, CancellationToken token = default)
    {
        var complaint = await complaintRepository.GetAsync(resource.ComplaintId, token: token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        complaintManager.Delete(complaint, resource.Comment, currentUser);
        await complaintRepository.UpdateAsync(complaint, autoSave: false, token: token).ConfigureAwait(false);
        await AddTransitionAsync(complaint, TransitionType.Deleted, currentUser, token, resource.Comment)
            .ConfigureAwait(false);
        await complaintRepository.SaveChangesAsync(token).ConfigureAwait(false);
    }

    public async Task RestoreAsync(ComplaintClosureDto resource, CancellationToken token = default)
    {
        var complaint = await complaintRepository.GetAsync(resource.ComplaintId, token: token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        complaintManager.Restore(complaint, currentUser);
        await complaintRepository.UpdateAsync(complaint, autoSave: false, token: token).ConfigureAwait(false);
        await AddTransitionAsync(complaint, TransitionType.Restored, currentUser, token, resource.Comment)
            .ConfigureAwait(false);
        await complaintRepository.SaveChangesAsync(token).ConfigureAwait(false);
    }

    // Private complaint write methods

    private async Task AddTransitionAsync(Complaint complaint, TransitionType type, ApplicationUser? user,
        CancellationToken token, string? comment = null) =>
        await complaintRepository
            .InsertTransitionAsync(complaintManager.CreateTransition(complaint, type, user, comment), autoSave: false,
                token)
            .ConfigureAwait(false);

    private async Task AddAssignmentTransitionsAsync(Complaint complaint, ApplicationUser? currentUser,
        CancellationToken token, string? comment = null)
    {
        await AddTransitionAsync(complaint, TransitionType.Assigned, currentUser, token, comment).ConfigureAwait(false);
        if (complaint.CurrentOwner != null && complaint.CurrentOwner == currentUser)
            await AddTransitionAsync(complaint, TransitionType.Accepted, currentUser, token, comment)
                .ConfigureAwait(false);
    }

    internal async Task<Complaint> CreateComplaintFromDtoAsync(ComplaintCreateDto resource,
        ApplicationUser? currentUser, CancellationToken token)
    {
        var complaint = complaintManager.Create(currentUser);
        await MapComplaintDetailsAsync(complaint, resource, token: token).ConfigureAwait(false);

        var office = await officeRepository.GetAsync(resource.OfficeId!.Value, Office.IncludeAssignor, token: token)
            .ConfigureAwait(false);
        var owner = resource.OwnerId is not null
            ? await userService.FindUserAsync(resource.OwnerId).ConfigureAwait(false)
            : null;

        complaintManager.Assign(complaint, office, owner, currentUser);
        return complaint;
    }

    private async Task MapComplaintDetailsAsync(Complaint complaint, IComplaintCommandDto resource,
        CancellationToken token)
    {
        // Properties: Meta-data
        complaint.ReceivedDate = resource.ReceivedDate.ToDateTime(resource.ReceivedTime);
        complaint.ReceivedBy = await userService.GetUserAsync(resource.ReceivedById!).ConfigureAwait(false);

        // Properties: Caller
        complaint.CallerName = resource.CallerName;
        complaint.CallerRepresents = resource.CallerRepresents;
        complaint.CallerAddress = resource.CallerAddress;
        complaint.CallerPhoneNumber = resource.CallerPhoneNumber;
        complaint.CallerSecondaryPhoneNumber = resource.CallerSecondaryPhoneNumber;
        complaint.CallerTertiaryPhoneNumber = resource.CallerTertiaryPhoneNumber;
        complaint.CallerEmail = resource.CallerEmail;

        // Properties: Complaint details
        complaint.ComplaintNature = resource.ComplaintNature;
        complaint.ComplaintLocation = resource.ComplaintLocation;
        complaint.ComplaintDirections = resource.ComplaintDirections;
        complaint.ComplaintCity = resource.ComplaintCity;
        complaint.ComplaintCounty = resource.ComplaintCounty;
        complaint.PrimaryConcern = await concernRepository.GetAsync(resource.PrimaryConcernId, token: token)
            .ConfigureAwait(false);
        complaint.SecondaryConcern =
            resource.SecondaryConcernId == null || resource.SecondaryConcernId == resource.PrimaryConcernId
                ? null
                : await concernRepository.GetAsync(resource.SecondaryConcernId.Value, token: token).ConfigureAwait(false);

        // Properties: Source
        complaint.SourceFacilityIdNumber = resource.SourceFacilityIdNumber;
        complaint.SourceFacilityName = resource.SourceFacilityName;
        complaint.SourceContactName = resource.SourceContactName;
        complaint.SourceAddress = resource.SourceAddress;
        complaint.SourcePhoneNumber = resource.SourcePhoneNumber;
        complaint.SourceSecondaryPhoneNumber = resource.SourceSecondaryPhoneNumber;
        complaint.SourceTertiaryPhoneNumber = resource.SourceTertiaryPhoneNumber;
        complaint.SourceEmail = resource.SourceEmail;
    }

    #region IDisposable,  IAsyncDisposable

    public void Dispose()
    {
        complaintRepository.Dispose();
        concernRepository.Dispose();
        officeRepository.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await complaintRepository.DisposeAsync().ConfigureAwait(false);
        await concernRepository.DisposeAsync().ConfigureAwait(false);
        await officeRepository.DisposeAsync().ConfigureAwait(false);
    }

    #endregion
}
