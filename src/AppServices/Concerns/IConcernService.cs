using GaEpd.AppLibrary.ListItems;

namespace Cts.AppServices.Concerns;

public interface IConcernService : IDisposable
{
    Task<ConcernUpdateDto?> FindForUpdateAsync(Guid id, CancellationToken token = default);
    Task<IReadOnlyList<ConcernViewDto>> GetListAsync(CancellationToken token = default);
    Task<IReadOnlyList<ListItem>> GetActiveListItemsAsync(CancellationToken token = default);
    Task<Guid> CreateAsync(string name, CancellationToken token = default);
    Task UpdateAsync(Guid id, ConcernUpdateDto resource, CancellationToken token = default);
}
