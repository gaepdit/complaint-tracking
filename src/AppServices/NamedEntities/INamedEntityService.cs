using GaEpd.AppLibrary.ListItems;

namespace Cts.AppServices.NamedEntities;

public interface INamedEntityService<TViewDto, TUpdateDto> : IDisposable, IAsyncDisposable
{
    Task<TUpdateDto?> FindForUpdateAsync(Guid id, CancellationToken token = default);
    Task<IReadOnlyList<TViewDto>> GetListAsync(CancellationToken token = default);
    Task<IReadOnlyList<ListItem>> GetAsListItemsAsync(bool includeInactive = false, CancellationToken token = default);
    Task<Guid> CreateAsync(string name, CancellationToken token = default);
    Task UpdateAsync(Guid id, TUpdateDto resource, CancellationToken token = default);
}
