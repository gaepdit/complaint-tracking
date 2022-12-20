using AutoMapper;
using Cts.AppServices.Complaints.Dto;
using Cts.Domain.AppLibraryExtra;
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

    public async Task<ComplaintPublicViewDto?> GetPublicViewAsync(int id, CancellationToken token = default)
    {
        var predicate = PredicateBuilder.True<Complaint>()
            .WithId(id)
            .IsPublic();

        return _mapper.Map<ComplaintPublicViewDto>(await _repository.FindAsync(predicate, token));
    }

    public async Task<IPaginatedResult<ComplaintPublicViewDto>> PublicSearchAsync(
        ComplaintPublicSearchDto spec,
        PaginatedRequest paging,
        CancellationToken token = default)
    {
        var predicate = ComplaintFilters.PublicSearchPredicate(spec);
        var resultsTask = _repository.GetPagedListAsync(predicate, paging, token);
        var countTask = _repository.GetCountAsync(predicate, token);

        var list = _mapper.Map<List<ComplaintPublicViewDto>>(await resultsTask);

        return new PaginatedResult<ComplaintPublicViewDto>(list, await countTask, paging);
    }

    public void Dispose() => _repository.Dispose();
}
