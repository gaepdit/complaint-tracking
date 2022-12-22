using AutoMapper;
using Cts.AppServices.Complaints.Dto;
using Cts.Domain.Complaints;
using GaEpd.AppLibrary.Pagination;

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

    public async Task<ComplaintPublicViewDto?> GetPublicViewAsync(int id, CancellationToken token = default) =>
        _mapper.Map<ComplaintPublicViewDto>(await _repository.FindAsync(ComplaintFilters.PublicIdPredicate(id), token));

    public async Task<bool> PublicComplaintExistsAsync(int id, CancellationToken token = default) =>
        await _repository.ExistsAsync(ComplaintFilters.PublicIdPredicate(id), token);

    public async Task<IPaginatedResult<ComplaintPublicViewDto>> PublicSearchAsync(
        ComplaintPublicSearchDto spec,
        PaginatedRequest paging,
        CancellationToken token = default)
    {
        var predicate = ComplaintFilters.PublicSearchPredicate(spec);

        var count = await _repository.GetCountAsync(predicate, token);

        var list = count > 0
            ? _mapper.Map<List<ComplaintPublicViewDto>>(await _repository.GetPagedListAsync(predicate, paging, token))
            : new List<ComplaintPublicViewDto>();

        return new PaginatedResult<ComplaintPublicViewDto>(list, count, paging);
    }

    public void Dispose() => _repository.Dispose();
}
