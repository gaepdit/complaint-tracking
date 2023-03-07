using AutoMapper;
using Cts.AppServices.Attachments;
using Cts.AppServices.ComplaintActions;
using Cts.AppServices.Complaints.Dto;
using Cts.AppServices.ComplaintTransitions;
using Cts.AppServices.UserServices;
using Cts.Domain.Complaints;
using GaEpd.AppLibrary.Pagination;
using Microsoft.AspNetCore.Http;

namespace Cts.AppServices.Complaints;

public sealed class ComplaintAppService : IComplaintAppService
{
    private readonly IComplaintRepository _repository;
    private readonly IComplaintManager _manager;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    public ComplaintAppService(
        IComplaintRepository repository,
        IComplaintManager manager,
        IMapper mapper,
        IUserService userService)
    {
        _repository = repository;
        _manager = manager;
        _mapper = mapper;
        _userService = userService;
    }

    public async Task<ComplaintPublicViewDto?> GetPublicAsync(int id, CancellationToken token = default)
    {
        var item = _mapper.Map<ComplaintPublicViewDto>(
            await _repository.FindAsync(ComplaintFilters.PublicIdPredicate(id), token));
        if (item is null) return item;

        item.ComplaintActions = await GetPublicComplaintActionsAsync(id, token);
        item.Attachments = await GetPublicAttachmentsAsync(id, token);
        return item;
    }

    private async Task<IReadOnlyList<ComplaintActionPublicViewDto>> GetPublicComplaintActionsAsync(
        int complaintId, CancellationToken token) =>
        _mapper.Map<IReadOnlyList<ComplaintActionPublicViewDto>>(
            await _repository.GetComplaintActionsListAsync(
                ComplaintActionFilters.PublicIdPredicate(complaintId), token));

    private async Task<IReadOnlyList<AttachmentPublicViewDto>> GetPublicAttachmentsAsync(int complaintId,
        CancellationToken token = default) =>
        _mapper.Map<IReadOnlyList<AttachmentPublicViewDto>>(
            await _repository.GetAttachmentsListAsync(AttachmentFilters.PublicIdPredicate(complaintId), token));

    public async Task<bool> PublicExistsAsync(int id, CancellationToken token = default) =>
        await _repository.ExistsAsync(ComplaintFilters.PublicIdPredicate(id), token);

    public async Task<IPaginatedResult<ComplaintSearchResultDto>> PublicSearchAsync(
        ComplaintPublicSearchDto spec,
        PaginatedRequest paging,
        CancellationToken token = default)
    {
        var predicate = ComplaintFilters.PublicSearchPredicate(spec);

        var count = await _repository.CountAsync(predicate, token);

        var list = count > 0
            ? _mapper.Map<List<ComplaintSearchResultDto>>(await _repository.GetPagedListAsync(predicate, paging, token))
            : new List<ComplaintSearchResultDto>();

        return new PaginatedResult<ComplaintSearchResultDto>(list, count, paging);
    }

    public async Task<AttachmentPublicViewDto?> GetPublicAttachmentAsync(Guid id, CancellationToken token = default)
    {
        var attachment = await _repository.FindAttachmentAsync(id, token);
        if (attachment is null || attachment.IsDeleted || attachment.Complaint is null) return null;
        var complaint = new List<Complaint> { attachment.Complaint }
            .SingleOrDefault(ComplaintFilters.IsPublicPredicate().Compile());
        return complaint is null ? null : _mapper.Map<AttachmentPublicViewDto>(attachment);
    }

    public async Task<ComplaintViewDto?> GetAsync(int id, CancellationToken token = default)
    {
        var item = _mapper.Map<ComplaintViewDto>(await _repository.FindAsync(id, token));
        if (item is null) return item;

        item.ComplaintActions = await GetActionsAsync(id, token);
        item.Attachments = await GetAttachmentsAsync(id, token);
        item.ComplaintTransitions = await GetTransitionsAsync(id, token);
        return item;
    }

    private async Task<IReadOnlyList<ComplaintActionViewDto>> GetActionsAsync(
        int complaintId, CancellationToken token) =>
        _mapper.Map<IReadOnlyList<ComplaintActionViewDto>>(
            await _repository.GetComplaintActionsListAsync(ComplaintActionFilters.IdPredicate(complaintId), token));

    private async Task<IReadOnlyList<AttachmentViewDto>> GetAttachmentsAsync(
        int complaintId, CancellationToken token) =>
        _mapper.Map<IReadOnlyList<AttachmentViewDto>>(
            await _repository.GetAttachmentsListAsync(AttachmentFilters.IdPredicate(complaintId), token));

    private async Task<IReadOnlyList<ComplaintTransitionViewDto>?> GetTransitionsAsync(
        int complaintId, CancellationToken token) =>
        _mapper.Map<IReadOnlyList<ComplaintTransitionViewDto>>(
            await _repository.GetComplaintTransitionsListAsync(complaintId, token));

    public async Task<bool> ExistsAsync(int id, CancellationToken token = default) =>
        await _repository.ExistsAsync(id, token);

    public async Task<IPaginatedResult<ComplaintSearchResultDto>> SearchAsync(
        ComplaintSearchDto spec,
        PaginatedRequest paging,
        CancellationToken token = default)
    {
        var predicate = ComplaintFilters.SearchPredicate(spec);

        var count = await _repository.CountAsync(predicate, token);

        var list = count > 0
            ? _mapper.Map<List<ComplaintSearchResultDto>>(await _repository.GetPagedListAsync(predicate, paging, token))
            : new List<ComplaintSearchResultDto>();

        return new PaginatedResult<ComplaintSearchResultDto>(list, count, paging);
    }

    public async Task<AttachmentViewDto?> GetAttachmentAsync(Guid id, CancellationToken token = default)
    {
        var attachment = await _repository.FindAttachmentAsync(id, token);
        return attachment is null || attachment.Complaint is null
            ? null
            : _mapper.Map<AttachmentViewDto>(attachment);
    }

    public async Task<int> CreateAsync(ComplaintCreateDto resource, CancellationToken token = default)
    {
        var item = _manager.Create();
        item.SetCreator((await _userService.GetCurrentUserAsync())?.Id);
        await _repository.InsertAsync(item, token: token);
        return item.Id;
    }

    public Task SaveAttachmentAsync(IFormFile file, CancellationToken token = default)
    {
        // var fileName = file.FileName.Trim();
        // var fileExtension = Path.GetExtension(fileName);
        // var fileId = Guid.NewGuid();
        // var saveFileName = string.Concat(fileId.ToString(), fileExtension);
        throw new NotImplementedException();
    }

    public void Dispose() => _repository.Dispose();
}
