using GaEpd.Library.Domain.Entities;

namespace Cts.LocalRepository.Repositories;

public abstract class BaseRepository<TEntity, TKey> : BaseReadOnlyRepository<TEntity, TKey>
    where TEntity : IEntity<TKey>
    where TKey : IEquatable<TKey>
{
    protected BaseRepository(IEnumerable<TEntity> items) : base(items) { }

    public Task InsertAsync(TEntity entity, bool autoSave = false, CancellationToken token = default)
    {
        Items.Add(entity);
        return Task.CompletedTask;
    }

    public async Task UpdateAsync(TEntity entity, bool autoSave = false, CancellationToken token = default)
    {
        var item = await GetAsync(entity.Id, token);
        Items.Remove(item);
        Items.Add(entity);
    }

    public async Task DeleteAsync(TKey id, bool autoSave = false, CancellationToken token = default) =>
        Items.Remove(await GetAsync(id, token));
}
