using AutoMapper;
using Cts.Domain.Complaints;

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
        var item = await _repository.FindAsync(e => e.Id == id && !e.IsDeleted && e.ComplaintClosed, token);
        return _mapper.Map<ComplaintPublicViewDto>(item);
    }

    public void Dispose() => _repository.Dispose();
}
