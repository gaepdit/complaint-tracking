namespace Cts.AppServices.Concerns;

public interface IConcernAppService : IDisposable
{
    Task<ConcernUpdateDto?> FindForUpdateAsync(Guid id, CancellationToken token = default);
    Task<IReadOnlyList<ConcernViewDto>> GetListAsync(CancellationToken token = default);
    Task<Guid> CreateAsync(string name, CancellationToken token = default);
    Task UpdateAsync(ConcernUpdateDto resource, CancellationToken token = default);
}
