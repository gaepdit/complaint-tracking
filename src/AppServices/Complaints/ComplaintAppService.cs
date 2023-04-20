using AutoMapper;
using Cts.AppServices.Attachments;
using Cts.AppServices.ComplaintActions;
using Cts.AppServices.Complaints.Dto;
using Cts.AppServices.ComplaintTransitions;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities.Complaints;
using Cts.Domain.Entities.Concerns;
using Cts.Domain.Entities.Offices;
using GaEpd.AppLibrary.Pagination;
using Microsoft.AspNetCore.Http;

namespace Cts.AppServices.Complaints;

public sealed class ComplaintAppService : IComplaintAppService
{
    private readonly IComplaintRepository _complaints;
    private readonly IConcernRepository _concerns;
    private readonly IOfficeRepository _offices;
    private readonly IComplaintManager _complaintManager;
    private readonly IMapper _mapper;
    private readonly IUserService _users;

    public ComplaintAppService(
        IComplaintRepository complaints,
        IConcernRepository concerns,
        IOfficeRepository offices,
        IComplaintManager complaintManager,
        IMapper mapper,
        IUserService users)
    {
        _complaints = complaints;
        _concerns = concerns;
        _offices = offices;
        _complaintManager = complaintManager;
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

    private async Task<IReadOnlyList<AttachmentPublicViewDto>> GetPublicAttachmentsAsync(int complaintId,
        CancellationToken token = default) =>
        _mapper.Map<IReadOnlyList<AttachmentPublicViewDto>>(
            await _complaints.GetAttachmentsListAsync(AttachmentFilters.PublicIdPredicate(complaintId), token));

    public async Task<bool> PublicExistsAsync(int id, CancellationToken token = default) =>
        await _complaints.ExistsAsync(ComplaintFilters.PublicIdPredicate(id), token);

    public async Task<IPaginatedResult<ComplaintSearchResultDto>> PublicSearchAsync(
        ComplaintPublicSearchDto spec,
        PaginatedRequest paging,
        CancellationToken token = default)
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
        ComplaintSearchDto spec,
        PaginatedRequest paging,
        CancellationToken token = default)
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
        var item = await CreateComplaintFromDtoAsync(resource, token);

        // TODO: Save transitions.
        await _complaints.InsertAsync(item, autoSave: true, token: token);
        return item.Id;
    }

    internal async Task<Complaint> CreateComplaintFromDtoAsync(ComplaintCreateDto resource, CancellationToken token)
    {
        var item = await _complaintManager.CreateNewComplaintAsync();
        var currentUserId = (await _users.GetCurrentUserAsync())?.Id;
        item.SetCreator(currentUserId);

        // Properties: Meta-data
        if (!string.IsNullOrEmpty(currentUserId))
            item.EnteredBy = await _users.GetUserAsync(currentUserId);
        item.ReceivedDate = DateTime.SpecifyKind(resource.ReceivedDate, DateTimeKind.Local)
            .Add(resource.ReceivedTime.TimeOfDay);
        item.ReceivedBy = await _users.GetUserAsync(resource.ReceivedById!);

        // Properties: Caller
        item.CallerName = resource.CallerName;
        item.CallerRepresents = resource.CallerRepresents;
        item.CallerAddress = resource.CallerAddress;
        item.CallerPhoneNumber = resource.CallerPhoneNumber;
        item.CallerSecondaryPhoneNumber = resource.CallerSecondaryPhoneNumber;
        item.CallerTertiaryPhoneNumber = resource.CallerTertiaryPhoneNumber;
        item.CallerEmail = resource.CallerEmail;

        // Properties: Complaint details
        item.ComplaintNature = resource.ComplaintNature;
        item.ComplaintLocation = resource.ComplaintLocation;
        item.ComplaintDirections = resource.ComplaintDirections;
        item.ComplaintCity = resource.ComplaintCity;
        item.ComplaintCounty = resource.ComplaintCounty;
        item.PrimaryConcern = await _concerns.GetAsync(resource.PrimaryConcernId, token);
        item.SecondaryConcern = resource.SecondaryConcernId is null
            ? null
            : await _concerns.FindAsync(resource.SecondaryConcernId.Value, token);

        // Properties: Source
        item.SourceFacilityIdNumber = resource.SourceFacilityIdNumber;
        item.SourceFacilityName = resource.SourceFacilityName;
        item.SourceContactName = resource.SourceContactName;
        item.SourceAddress = resource.SourceAddress;
        item.SourcePhoneNumber = resource.SourcePhoneNumber;
        item.SourceSecondaryPhoneNumber = resource.SourceSecondaryPhoneNumber;
        item.SourceTertiaryPhoneNumber = resource.SourceTertiaryPhoneNumber;
        item.SourceEmail = resource.SourceEmail;

        // Properties: Assignment/History
        item.CurrentOffice = await _offices.GetAsync(resource.CurrentOfficeId!.Value, token);
        if (resource.CurrentOwnerId != null)
        {
            item.CurrentOwner = await _users.FindUserAsync(resource.CurrentOwnerId);
            item.CurrentOwnerAssignedDate = DateTimeOffset.Now;
            item.CurrentOwnerAcceptedDate = resource.CurrentOwnerId.Equals(currentUserId) ? DateTimeOffset.Now : null;
        }

        return item;
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
