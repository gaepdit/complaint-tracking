using AutoMapper;
using Cts.AppServices.Attachments;
using Cts.AppServices.Complaints.Dto;
using Cts.Domain.Complaints;
using GaEpd.AppLibrary.Pagination;
using Microsoft.AspNetCore.Http;

namespace Cts.AppServices.Complaints;

public sealed class ComplaintAppService : IComplaintAppService
{
    private readonly IComplaintRepository _repository;
    private readonly IMapper _mapper;

    public ComplaintAppService(IComplaintRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ComplaintPublicViewDto?> GetPublicViewAsync(int id, CancellationToken token = default)
    {
        var item = _mapper.Map<ComplaintPublicViewDto>(
            await _repository.FindAsync(ComplaintFilters.PublicIdPredicate(id), token));
        if (item is not null) item.Attachments = await GetPublicAttachmentsAsync(id, token);
        return item;
    }

    private async Task<IReadOnlyList<AttachmentPublicViewDto>> GetPublicAttachmentsAsync(int complaintId,
        CancellationToken token = default) =>
        _mapper.Map<IReadOnlyList<AttachmentPublicViewDto>>(
            await _repository.GetAttachmentsListAsync(AttachmentFilters.PublicIdPredicate(complaintId), token));

    public async Task<bool> PublicComplaintExistsAsync(int id, CancellationToken token = default) =>
        await _repository.ExistsAsync(ComplaintFilters.PublicIdPredicate(id), token);

    public async Task<IPaginatedResult<ComplaintPublicViewDto>> PublicSearchAsync(
        ComplaintPublicSearchDto spec,
        PaginatedRequest paging,
        CancellationToken token = default)
    {
        var predicate = ComplaintFilters.PublicSearchPredicate(spec);

        var count = await _repository.CountAsync(predicate, token);

        var list = count > 0
            ? _mapper.Map<List<ComplaintPublicViewDto>>(await _repository.GetPagedListAsync(predicate, paging, token))
            : new List<ComplaintPublicViewDto>();

        return new PaginatedResult<ComplaintPublicViewDto>(list, count, paging);
    }

    public async Task<AttachmentPublicViewDto?> GetPublicAttachmentAsync(Guid id, CancellationToken token = default)
    {
        var attachment = await _repository.FindAttachmentAsync(id, token);
        if (attachment is null || attachment.IsDeleted) return null;
        var complaint = await _repository.FindAsync(ComplaintFilters.PublicIdPredicate(attachment.ComplaintId), token);
        return complaint is null ? null : _mapper.Map<AttachmentPublicViewDto>(attachment);
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
