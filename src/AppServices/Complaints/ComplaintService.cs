using AutoMapper;
using Cts.AppServices.Complaints.Dto;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities.Complaints;
using Cts.Domain.Entities.ComplaintTransitions;
using Cts.Domain.Entities.Concerns;
using Cts.Domain.Entities.Offices;
using Cts.Domain.Identity;
using GaEpd.AppLibrary.Pagination;

namespace Cts.AppServices.Complaints;

public sealed class ComplaintService(
    IComplaintRepository complaintRepository,
    IComplaintManager complaintManager,
    IConcernRepository concernRepository,
    IOfficeRepository officeRepository,
    IComplaintTransitionManager transitionManager,
    IMapper mapper,
    IUserService userService)
    : IComplaintService
{
    public async Task<ComplaintPublicViewDto?> FindPublicAsync(int id, CancellationToken token = default)
    {
        var complaint = await complaintRepository
            .FindIncludeAllAsync(ComplaintFilters.PublicIdPredicate(id), token: token).ConfigureAwait(false);
        return complaint is null ? null : mapper.Map<ComplaintPublicViewDto>(complaint);
    }

    public Task<bool> PublicExistsAsync(int id, CancellationToken token = default) =>
        complaintRepository.ExistsAsync(ComplaintFilters.PublicIdPredicate(id), token);

    public async Task<IPaginatedResult<ComplaintSearchResultDto>> PublicSearchAsync(
        ComplaintPublicSearchDto spec, PaginatedRequest paging, CancellationToken token = default)
    {
        var predicate = ComplaintFilters.PublicSearchPredicate(spec);

        var count = await complaintRepository.CountAsync(predicate, token).ConfigureAwait(false);

        var list = count > 0
            ? mapper.Map<List<ComplaintSearchResultDto>>(await complaintRepository
                .GetPagedListAsync(predicate, paging, token).ConfigureAwait(false))
            : [];

        return new PaginatedResult<ComplaintSearchResultDto>(list, count, paging);
    }


    public async Task<ComplaintViewDto?> FindAsync(int id, bool includeDeletedActions = false,
        CancellationToken token = default)
    {
        var complaint = await complaintRepository.FindIncludeAllAsync(id, includeDeletedActions, token)
            .ConfigureAwait(false);
        return complaint is null ? null : mapper.Map<ComplaintViewDto>(complaint);
    }

    public async Task<ComplaintUpdateDto?> FindForUpdateAsync(int id, CancellationToken token = default) =>
        mapper.Map<ComplaintUpdateDto>(await complaintRepository.FindAsync(e =>
            e.Id == id && !e.IsDeleted && !e.ComplaintClosed, token).ConfigureAwait(false));

    public async Task<bool> ExistsAsync(int id, CancellationToken token = default) =>
        await complaintRepository.ExistsAsync(id, token).ConfigureAwait(false);

    public async Task<IPaginatedResult<ComplaintSearchResultDto>> SearchAsync(
        ComplaintSearchDto spec, PaginatedRequest paging, CancellationToken token = default)
    {
        var predicate = ComplaintFilters.SearchPredicate(spec);

        var count = await complaintRepository.CountAsync(predicate, token).ConfigureAwait(false);

        var list = count > 0
            ? mapper.Map<List<ComplaintSearchResultDto>>(
                await complaintRepository.GetPagedListAsync(predicate, paging, token).ConfigureAwait(false))
            : [];

        return new PaginatedResult<ComplaintSearchResultDto>(list, count, paging);
    }

    public async Task<int> CreateAsync(ComplaintCreateDto resource, CancellationToken token = default)
    {
        var user = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        var complaint = await CreateComplaintFromDtoAsync(resource, user, token).ConfigureAwait(false);
        await complaintRepository.InsertAsync(complaint, autoSave: false, token: token).ConfigureAwait(false);

        await AddTransitionAsync(complaint, TransitionType.New, user, token).ConfigureAwait(false);

        if (resource.CurrentOwnerId is not null)
            await AddTransitionAsync(complaint, TransitionType.Assigned, user, token).ConfigureAwait(false);

        if (resource.CurrentOwnerId is not null && complaint.CurrentOwner == user)
            await AddTransitionAsync(complaint, TransitionType.Accepted, user, token).ConfigureAwait(false);

        await complaintRepository.SaveChangesAsync(token).ConfigureAwait(false);
        return complaint.Id;
    }

    public async Task UpdateAsync(int id, ComplaintUpdateDto resource, CancellationToken token = default)
    {
        var complaint = await complaintRepository.GetAsync(id, token).ConfigureAwait(false);
        complaint.SetUpdater((await userService.GetCurrentUserAsync().ConfigureAwait(false))?.Id);
        await FillInComplaintDetailsAsync(complaint, resource, token).ConfigureAwait(false);
        await complaintRepository.UpdateAsync(complaint, token: token).ConfigureAwait(false);
    }

    private async Task AddTransitionAsync(
        Complaint complaint, TransitionType type, ApplicationUser? user, CancellationToken token) =>
        await complaintRepository.InsertTransitionAsync(transitionManager.Create(complaint, type, user, user?.Id),
            autoSave: false,
            token).ConfigureAwait(false);

    internal async Task<Complaint> CreateComplaintFromDtoAsync(
        ComplaintCreateDto resource, ApplicationUser? currentUser, CancellationToken token)
    {
        var complaint = complaintManager.Create(currentUser);
        await FillInComplaintDetailsAsync(complaint, resource, token).ConfigureAwait(false);

        // Properties: Assignment
        complaint.CurrentOffice = await officeRepository.GetAsync(resource.CurrentOfficeId!.Value, token)
            .ConfigureAwait(false);

        if (resource.CurrentOwnerId == null) return complaint;

        complaint.CurrentOwner = await userService.FindUserAsync(resource.CurrentOwnerId).ConfigureAwait(false);
        complaint.CurrentOwnerAssignedDate = DateTimeOffset.Now;

        if (resource.CurrentOwnerId == null || !resource.CurrentOwnerId.Equals(currentUser?.Id)) return complaint;

        complaint.CurrentOwnerAcceptedDate = DateTimeOffset.Now;
        complaint.Status = ComplaintStatus.UnderInvestigation;

        return complaint;
    }

    private async Task FillInComplaintDetailsAsync(Complaint complaint, IComplaintDtoDetails resource,
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
        complaint.PrimaryConcern = await concernRepository.GetAsync(resource.PrimaryConcernId, token)
            .ConfigureAwait(false);
        if (resource.SecondaryConcernId is not null)
        {
            complaint.SecondaryConcern = await concernRepository.FindAsync(resource.SecondaryConcernId.Value, token)
                .ConfigureAwait(false);
        }

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

    public async Task DeleteAsync(int complaintId, CancellationToken token = default)
    {
        var complaint = await complaintRepository.GetAsync(complaintId, token).ConfigureAwait(false);
        complaint.SetDeleted((await userService.GetCurrentUserAsync().ConfigureAwait(false))?.Id);
        await complaintRepository.UpdateAsync(complaint, token: token).ConfigureAwait(false);
    }

    public async Task RestoreAsync(int complaintId, CancellationToken token = default)
    {
        var complaint = await complaintRepository.GetAsync(complaintId, token).ConfigureAwait(false);
        complaint.SetNotDeleted();
        await complaintRepository.UpdateAsync(complaint, token: token).ConfigureAwait(false);
    }

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
}
