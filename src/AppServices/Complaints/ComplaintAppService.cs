using AutoMapper;
using Cts.AppServices.Attachments;
using Cts.AppServices.ComplaintActions;
using Cts.AppServices.Complaints.Dto;
using Cts.AppServices.ComplaintTransitions;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities.Complaints;
using Cts.Domain.Entities.ComplaintTransitions;
using Cts.Domain.Entities.Concerns;
using Cts.Domain.Entities.Offices;
using Cts.Domain.Identity;
using GaEpd.AppLibrary.Pagination;
using Microsoft.AspNetCore.Http;

namespace Cts.AppServices.Complaints;

public sealed class ComplaintAppService : IComplaintAppService
{
    private readonly IComplaintRepository _complaints;
    private readonly IComplaintManager _complaintManager;
    private readonly IConcernRepository _concerns;
    private readonly IOfficeRepository _offices;
    private readonly IComplaintTransitionRepository _transitions;
    private readonly IComplaintTransitionManager _transitionManager;
    private readonly IMapper _mapper;
    private readonly IUserService _users;

    public ComplaintAppService(IComplaintRepository complaints,
        IComplaintManager complaintManager,
        IConcernRepository concerns,
        IOfficeRepository offices,
        IComplaintTransitionRepository transitions,
        IComplaintTransitionManager transitionManager,
        IMapper mapper,
        IUserService users)
    {
        _complaints = complaints;
        _concerns = concerns;
        _offices = offices;
        _complaintManager = complaintManager;
        _transitions = transitions;
        _transitionManager = transitionManager;
        _mapper = mapper;
        _users = users;
    }

    public async Task<ComplaintPublicViewDto?> GetPublicAsync(int id, CancellationToken token = default)
    {
        var item = _mapper.Map<ComplaintPublicViewDto>(
            await _complaints.FindAsync(ComplaintFilters.PublicIdPredicate(id), token));
        if (item is null) return item;

        item.ComplaintActions = await GetPublicComplaintActionsAsync(id, token);
        item.Attachments = await GetPublicAttachmentsAsync(id, token);
        return item;
    }

    private async Task<IReadOnlyList<ComplaintActionPublicViewDto>> GetPublicComplaintActionsAsync(
        int complaintId, CancellationToken token) =>
        _mapper.Map<IReadOnlyList<ComplaintActionPublicViewDto>>(
            await _complaints.GetComplaintActionsListAsync(
                ComplaintActionFilters.PublicIdPredicate(complaintId), token));

    private async Task<IReadOnlyList<AttachmentPublicViewDto>> GetPublicAttachmentsAsync(
        int complaintId, CancellationToken token = default) =>
        _mapper.Map<IReadOnlyList<AttachmentPublicViewDto>>(
            await _complaints.GetAttachmentsListAsync(AttachmentFilters.PublicIdPredicate(complaintId), token));

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

    public async Task<AttachmentPublicViewDto?> GetPublicAttachmentAsync(Guid id, CancellationToken token = default)
    {
        var attachment = await _complaints.FindAttachmentAsync(id, token);
        if (attachment is null || attachment.IsDeleted) return null;
        var complaint = new List<Complaint> { attachment.Complaint }
            .SingleOrDefault(ComplaintFilters.IsPublicPredicate().Compile());
        return complaint is null ? null : _mapper.Map<AttachmentPublicViewDto>(attachment);
    }

    public async Task<ComplaintViewDto?> GetAsync(int id, CancellationToken token = default)
    {
        var item = _mapper.Map<ComplaintViewDto>(await _complaints.FindAsync(id, token));
        if (item is null) return item;

        item.ComplaintActions = await GetActionsAsync(id, token);
        item.Attachments = await GetAttachmentsAsync(id, token);
        item.ComplaintTransitions = await GetTransitionsAsync(id, token);
        return item;
    }

    private async Task<IReadOnlyList<ComplaintActionViewDto>> GetActionsAsync(
        int complaintId, CancellationToken token) =>
        _mapper.Map<IReadOnlyList<ComplaintActionViewDto>>(
            await _complaints.GetComplaintActionsListAsync(ComplaintActionFilters.IdPredicate(complaintId), token));

    private async Task<IReadOnlyList<AttachmentViewDto>> GetAttachmentsAsync(
        int complaintId, CancellationToken token) =>
        _mapper.Map<IReadOnlyList<AttachmentViewDto>>(
            await _complaints.GetAttachmentsListAsync(AttachmentFilters.IdPredicate(complaintId), token));

    private async Task<IReadOnlyList<ComplaintTransitionViewDto>?> GetTransitionsAsync(
        int complaintId, CancellationToken token) =>
        _mapper.Map<IReadOnlyList<ComplaintTransitionViewDto>>(
            await _complaints.GetComplaintTransitionsListAsync(complaintId, token));

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

    public async Task<AttachmentViewDto?> GetAttachmentAsync(Guid id, CancellationToken token = default)
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

    private async Task AddTransitionAsync(
        Complaint complaint, TransitionType type, ApplicationUser? user, CancellationToken token) =>
        await _transitions.InsertAsync(_transitionManager.Create(complaint, type, user), autoSave: false, token);

    internal async Task<Complaint> CreateComplaintFromDtoAsync(
        ComplaintCreateDto resource, string? currentUserId, CancellationToken token)
    {
        var complaint = await _complaintManager.CreateNewComplaintAsync();
        complaint.SetCreator(currentUserId);

        // Properties: Meta-data
        if (!string.IsNullOrEmpty(currentUserId))
            complaint.EnteredBy = await _users.GetUserAsync(currentUserId);
        complaint.ReceivedDate = DateTime.SpecifyKind(resource.ReceivedDate, DateTimeKind.Local)
            .Add(resource.ReceivedTime.TimeOfDay);
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

        // Properties: Assignment/History
        complaint.CurrentOffice = await _offices.GetAsync(resource.CurrentOfficeId!.Value, token);

        if (resource.CurrentOwnerId == null) return complaint;

        complaint.CurrentOwner = await _users.FindUserAsync(resource.CurrentOwnerId);
        complaint.CurrentOwnerAssignedDate = DateTimeOffset.Now;

        if (!resource.CurrentOwnerId.Equals(currentUserId)) return complaint;

        complaint.CurrentOwnerAcceptedDate = DateTimeOffset.Now;
        complaint.Status = ComplaintStatus.UnderInvestigation;

        return complaint;
    }

    public Task SaveAttachmentAsync(IFormFile file, CancellationToken token = default)
    {
        // var fileName = file.FileName.Trim();
        // var fileExtension = Path.GetExtension(fileName);
        // var fileId = Guid.NewGuid();
        // var saveFileName = string.Concat(fileId.ToString(), fileExtension);
        throw new NotImplementedException();
    }

    public void Dispose() => _complaints.Dispose();
}
