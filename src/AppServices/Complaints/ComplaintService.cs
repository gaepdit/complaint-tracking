using AutoMapper;
using Cts.AppServices.Attachments;
using Cts.AppServices.Complaints.Dto;
using Cts.AppServices.Staff.Dto;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities.Complaints;
using Cts.Domain.Entities.ComplaintTransitions;
using Cts.Domain.Entities.Concerns;
using Cts.Domain.Entities.Offices;
using Cts.Domain.Identity;
using GaEpd.AppLibrary.Pagination;
using Microsoft.AspNetCore.Http;

namespace Cts.AppServices.Complaints;

public sealed class ComplaintService(
    IComplaintRepository complaints,
    IComplaintManager manager,
    IConcernRepository concerns,
    IOfficeRepository offices,
    IComplaintTransitionManager transitions,
    IMapper mapper,
    IUserService users)
    : IComplaintService
{
    public async Task<ComplaintPublicViewDto?> FindPublicAsync(int id, CancellationToken token = default)
    {
        var complaint = await complaints.FindIncludeAllAsync(ComplaintFilters.PublicIdPredicate(id), token);
        return complaint is null ? null : mapper.Map<ComplaintPublicViewDto>(complaint);
    }

    public async Task<bool> PublicExistsAsync(int id, CancellationToken token = default) =>
        await complaints.ExistsAsync(ComplaintFilters.PublicIdPredicate(id), token);

    public async Task<IPaginatedResult<ComplaintSearchResultDto>> PublicSearchAsync(
        ComplaintPublicSearchDto spec, PaginatedRequest paging, CancellationToken token = default)
    {
        var predicate = ComplaintFilters.PublicSearchPredicate(spec);

        var count = await complaints.CountAsync(predicate, token);

        var list = count > 0
            ? mapper.Map<List<ComplaintSearchResultDto>>(await complaints.GetPagedListAsync(predicate, paging, token))
            : new List<ComplaintSearchResultDto>();

        return new PaginatedResult<ComplaintSearchResultDto>(list, count, paging);
    }

    public async Task<AttachmentPublicViewDto?> FindPublicAttachmentAsync(Guid id, CancellationToken token = default)
    {
        var attachment = await complaints.FindAttachmentAsync(id, token);
        if (attachment is null || attachment.IsDeleted) return null;
        var complaint = new List<Complaint> { attachment.Complaint }
            .SingleOrDefault(ComplaintFilters.IsPublicPredicate().Compile());
        return complaint is null ? null : mapper.Map<AttachmentPublicViewDto>(attachment);
    }

    public async Task<ComplaintViewDto?> FindAsync(int id, CancellationToken token = default)
    {
        var complaint = await complaints.FindIncludeAllAsync(ComplaintFilters.IdPredicate(id), token);
        if (complaint is null) return null;

        var view = mapper.Map<ComplaintViewDto>(complaint);
        return complaint is { IsDeleted: true, DeletedById: not null }
            ? view with { DeletedBy = mapper.Map<StaffViewDto>(await users.FindUserAsync(complaint.DeletedById)) }
            : view;
    }

    public async Task<ComplaintUpdateDto?> FindForUpdateAsync(int id, CancellationToken token = default) =>
        mapper.Map<ComplaintUpdateDto>(await complaints.FindAsync(e =>
            e.Id == id && !e.IsDeleted && !e.ComplaintClosed, token));

    public async Task<bool> ExistsAsync(int id, CancellationToken token = default) =>
        await complaints.ExistsAsync(id, token);

    public async Task<IPaginatedResult<ComplaintSearchResultDto>> SearchAsync(
        ComplaintSearchDto spec, PaginatedRequest paging, CancellationToken token = default)
    {
        var predicate = ComplaintFilters.SearchPredicate(spec);

        var count = await complaints.CountAsync(predicate, token);

        var list = count > 0
            ? mapper.Map<List<ComplaintSearchResultDto>>(await complaints.GetPagedListAsync(predicate, paging, token))
            : new List<ComplaintSearchResultDto>();

        return new PaginatedResult<ComplaintSearchResultDto>(list, count, paging);
    }

    public async Task<AttachmentViewDto?> FindAttachmentAsync(Guid id, CancellationToken token = default)
    {
        var attachment = await complaints.FindAttachmentAsync(id, token);
        return mapper.Map<AttachmentViewDto>(attachment);
    }

    public async Task<int> CreateAsync(ComplaintCreateDto resource, CancellationToken token = default)
    {
        var user = await users.GetCurrentUserAsync();
        var complaint = await CreateComplaintFromDtoAsync(resource, user?.Id, token);
        await complaints.InsertAsync(complaint, autoSave: false, token: token);

        await AddTransitionAsync(complaint, TransitionType.New, user, token);

        if (resource.CurrentOwnerId is not null)
            await AddTransitionAsync(complaint, TransitionType.Assigned, user, token);

        if (resource.CurrentOwnerId is not null && complaint.CurrentOwner == user)
            await AddTransitionAsync(complaint, TransitionType.Accepted, user, token);

        await complaints.SaveChangesAsync(token);
        return complaint.Id;
    }

    public async Task UpdateAsync(int id, ComplaintUpdateDto resource, CancellationToken token = default)
    {
        var complaint = await complaints.GetAsync(id, token);
        complaint.SetUpdater((await users.GetCurrentUserAsync())?.Id);
        await FillInComplaintDetailsAsync(complaint, resource, token);
        await complaints.UpdateAsync(complaint, token: token);
    }

    private async Task AddTransitionAsync(
        Complaint complaint, TransitionType type, ApplicationUser? user, CancellationToken token) =>
        await complaints.InsertTransitionAsync(transitions.Create(complaint, type, user, user?.Id), autoSave: false,
            token);

    internal async Task<Complaint> CreateComplaintFromDtoAsync(
        ComplaintCreateDto resource, string? currentUserId, CancellationToken token)
    {
        var complaint = manager.CreateNewComplaint(currentUserId);
        await FillInComplaintDetailsAsync(complaint, resource, token);

        // Auditing data
        if (!string.IsNullOrEmpty(currentUserId))
            complaint.EnteredBy = await users.GetUserAsync(currentUserId);

        // Properties: Assignment
        complaint.CurrentOffice = await offices.GetAsync(resource.CurrentOfficeId!.Value, token);

        if (resource.CurrentOwnerId == null) return complaint;

        complaint.CurrentOwner = await users.FindUserAsync(resource.CurrentOwnerId);
        complaint.CurrentOwnerAssignedDate = DateTimeOffset.Now;

        if (!resource.CurrentOwnerId.Equals(currentUserId)) return complaint;

        complaint.CurrentOwnerAcceptedDate = DateTimeOffset.Now;
        complaint.Status = ComplaintStatus.UnderInvestigation;

        return complaint;
    }

    private async Task FillInComplaintDetailsAsync(Complaint complaint, IComplaintDtoDetails resource,
        CancellationToken token)
    {
        // Properties: Meta-data
        complaint.ReceivedDate = resource.ReceivedDate.ToDateTime(resource.ReceivedTime);
        complaint.ReceivedBy = await users.GetUserAsync(resource.ReceivedById!);

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
        complaint.PrimaryConcern = await concerns.GetAsync(resource.PrimaryConcernId, token);
        if (resource.SecondaryConcernId is not null)
            complaint.SecondaryConcern = await concerns.FindAsync(resource.SecondaryConcernId.Value, token);

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

    public Task SaveAttachmentAsync(IFormFile file, CancellationToken token = default)
    {
#pragma warning disable S125
        // var fileName = file.FileName.Trim();
        // var fileExtension = Path.GetExtension(fileName);
        // var fileId = Guid.NewGuid();
        // var saveFileName = string.Concat(fileId.ToString(), fileExtension);
#pragma warning restore S125
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        complaints.Dispose();
        concerns.Dispose();
        offices.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await complaints.DisposeAsync();
        await concerns.DisposeAsync();
        await offices.DisposeAsync();
    }
}
