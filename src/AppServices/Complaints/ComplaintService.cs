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

public sealed class ComplaintService : IComplaintService
{
    private readonly IComplaintRepository _complaints;
    private readonly IComplaintManager _manager;
    private readonly IConcernRepository _concerns;
    private readonly IOfficeRepository _offices;
    private readonly IComplaintTransitionManager _transitions;
    private readonly IMapper _mapper;
    private readonly IUserService _users;

    public ComplaintService(IComplaintRepository complaints, IComplaintManager manager, IConcernRepository concerns,
        IOfficeRepository offices, IComplaintTransitionManager transitions, IMapper mapper, IUserService users)
    {
        _complaints = complaints;
        _concerns = concerns;
        _offices = offices;
        _manager = manager;
        _transitions = transitions;
        _mapper = mapper;
        _users = users;
    }

    public async Task<ComplaintPublicViewDto?> FindPublicAsync(int id, CancellationToken token = default)
    {
        var complaint = await _complaints.FindIncludeAllAsync(ComplaintFilters.PublicIdPredicate(id), token);
        return complaint is null ? null : _mapper.Map<ComplaintPublicViewDto>(complaint);
    }

    public async Task<bool> PublicExistsAsync(int id, CancellationToken token = default) =>
        await _complaints.ExistsAsync(ComplaintFilters.PublicIdPredicate(id), token);

    public async Task<IPaginatedResult<ComplaintSearchResultDto>> PublicSearchAsync(
        ComplaintPublicSearchDto spec, PaginatedRequest paging, CancellationToken token = default)
    {
        var predicate = ComplaintFilters.PublicSearchPredicate(spec);

        var count = await _complaints.CountAsync(predicate, token);

        var list = count > 0
            ? _mapper.Map<List<ComplaintSearchResultDto>>(await _complaints.GetPagedListAsync(predicate, paging, token))
            : new List<ComplaintSearchResultDto>();

        return new PaginatedResult<ComplaintSearchResultDto>(list, count, paging);
    }

    public async Task<AttachmentPublicViewDto?> FindPublicAttachmentAsync(Guid id, CancellationToken token = default)
    {
        var attachment = await _complaints.FindAttachmentAsync(id, token);
        if (attachment is null || attachment.IsDeleted) return null;
        var complaint = new List<Complaint> { attachment.Complaint }
            .SingleOrDefault(ComplaintFilters.IsPublicPredicate().Compile());
        return complaint is null ? null : _mapper.Map<AttachmentPublicViewDto>(attachment);
    }

    public async Task<ComplaintViewDto?> FindAsync(int id, CancellationToken token = default)
    {
        var complaint = await _complaints.FindIncludeAllAsync(ComplaintFilters.IdPredicate(id), token);
        if (complaint is null) return null;

        var view = _mapper.Map<ComplaintViewDto>(complaint);
        return complaint is { IsDeleted: true, DeletedById: not null }
            ? view with { DeletedBy = _mapper.Map<StaffViewDto>(await _users.FindUserAsync(complaint.DeletedById)) }
            : view;
    }

    public async Task<ComplaintUpdateDto?> FindForUpdateAsync(int id, CancellationToken token = default) =>
        _mapper.Map<ComplaintUpdateDto>(await _complaints.FindAsync(e =>
            e.Id == id && !e.IsDeleted && !e.ComplaintClosed, token));

    public async Task<bool> ExistsAsync(int id, CancellationToken token = default) =>
        await _complaints.ExistsAsync(id, token);

    public async Task<IPaginatedResult<ComplaintSearchResultDto>> SearchAsync(
        ComplaintSearchDto spec, PaginatedRequest paging, CancellationToken token = default)
    {
        var predicate = ComplaintFilters.SearchPredicate(spec);

        var count = await _complaints.CountAsync(predicate, token);

        var list = count > 0
            ? _mapper.Map<List<ComplaintSearchResultDto>>(await _complaints.GetPagedListAsync(predicate, paging, token))
            : new List<ComplaintSearchResultDto>();

        return new PaginatedResult<ComplaintSearchResultDto>(list, count, paging);
    }

    public async Task<AttachmentViewDto?> FindAttachmentAsync(Guid id, CancellationToken token = default)
    {
        var attachment = await _complaints.FindAttachmentAsync(id, token);
        return _mapper.Map<AttachmentViewDto>(attachment);
    }

    public async Task<int> CreateAsync(ComplaintCreateDto resource, CancellationToken token = default)
    {
        var user = await _users.GetCurrentUserAsync();
        var complaint = await CreateComplaintFromDtoAsync(resource, user?.Id, token);
        await _complaints.InsertAsync(complaint, autoSave: false, token: token);

        await AddTransitionAsync(complaint, TransitionType.New, user, token);

        if (resource.CurrentOwnerId is not null)
            await AddTransitionAsync(complaint, TransitionType.Assigned, user, token);

        if (resource.CurrentOwnerId is not null && complaint.CurrentOwner == user)
            await AddTransitionAsync(complaint, TransitionType.Accepted, user, token);

        await _complaints.SaveChangesAsync(token);
        return complaint.Id;
    }

    public async Task UpdateAsync(int id, ComplaintUpdateDto resource, CancellationToken token = default)
    {
        var complaint = await _complaints.GetAsync(id, token);
        complaint.SetUpdater((await _users.GetCurrentUserAsync())?.Id);
        await FillInComplaintDetailsAsync(complaint, resource, token);
        await _complaints.UpdateAsync(complaint, token: token);
    }

    private async Task AddTransitionAsync(
        Complaint complaint, TransitionType type, ApplicationUser? user, CancellationToken token) =>
        await _complaints.InsertTransitionAsync(_transitions.Create(complaint, type, user, user?.Id), autoSave: false,
            token);

    internal async Task<Complaint> CreateComplaintFromDtoAsync(
        ComplaintCreateDto resource, string? currentUserId, CancellationToken token)
    {
        var complaint = _manager.CreateNewComplaint(currentUserId);
        await FillInComplaintDetailsAsync(complaint, resource, token);

        // Auditing data
        if (!string.IsNullOrEmpty(currentUserId))
            complaint.EnteredBy = await _users.GetUserAsync(currentUserId);

        // Properties: Assignment
        complaint.CurrentOffice = await _offices.GetAsync(resource.CurrentOfficeId!.Value, token);

        if (resource.CurrentOwnerId == null) return complaint;

        complaint.CurrentOwner = await _users.FindUserAsync(resource.CurrentOwnerId);
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
        complaint.ReceivedBy = await _users.GetUserAsync(resource.ReceivedById!);

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
        complaint.PrimaryConcern = await _concerns.GetAsync(resource.PrimaryConcernId, token);
        if (resource.SecondaryConcernId is not null)
            complaint.SecondaryConcern = await _concerns.FindAsync(resource.SecondaryConcernId.Value, token);

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
        _complaints.Dispose();
        _concerns.Dispose();
        _offices.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _complaints.DisposeAsync();
        await _concerns.DisposeAsync();
        await _offices.DisposeAsync();
    }
}
