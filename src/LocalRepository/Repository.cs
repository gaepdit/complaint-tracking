using GaEpd.Library.Domain.Entities;
using GaEpd.Library.Domain.Repositories;
using GaEpd.Library.Pagination;

namespace Cts.LocalRepository;

public abstract class Repository<TEntity, TKey> : IRepository<TEntity, TKey>
    where TEntity : IEntity<TKey>
    where TKey : IEquatable<TKey>
{
    internal ICollection<TEntity> Items { get; }

    protected Repository(IEnumerable<TEntity> items) => Items = items.ToList();

    public Task<TEntity> GetAsync(TKey id, CancellationToken cancellationToken = default) =>
        Items.Any(e => e.Id.Equals(id))
            ? Task.FromResult(Items.Single(e => e.Id.Equals(id)))
            : throw new EntityNotFoundException(typeof(TEntity), id);

    public Task<TEntity?> FindAsync(TKey id, CancellationToken cancellationToken = default) =>
        Task.FromResult(Items.SingleOrDefault(e => e.Id.Equals(id)));

    public Task<TEntity?> FindAsync(Func<TEntity, bool> predicate,
        CancellationToken cancellationToken = default) =>
        Task.FromResult(Items.SingleOrDefault(predicate));

    public Task<IList<TEntity>> GetListAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(Items.ToList() as IList<TEntity>);

    public Task<IList<TEntity>> GetListAsync(Func<TEntity, bool> predicate,
        CancellationToken cancellationToken = default) =>
        Task.FromResult(Items.Where(predicate).ToList() as IList<TEntity>);

    public Task<IList<TEntity>> GetPagedListAsync(PaginatedRequest paging,
        CancellationToken cancellationToken = default) =>
        Task.FromResult(Items.Skip(paging.Skip).Take(paging.Take).ToList() as IList<TEntity>);

    public Task<IList<TEntity>> GetPagedListAsync(Func<TEntity, bool> predicate,
        PaginatedRequest paging, CancellationToken cancellationToken = default) =>
        Task.FromResult(Items.Where(predicate).Skip(paging.Skip).Take(paging.Take).ToList() as IList<TEntity>);

    public async Task<TEntity> InsertAsync(TEntity entity, bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        if (await FindAsync(entity.Id, cancellationToken) != null)
            throw new EntityAlreadyExistsException(typeof(TEntity), entity.Id);

        Items.Add(entity);
        return entity;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        var item = await GetAsync(entity.Id, cancellationToken);
        Items.Remove(item);
        Items.Add(entity);
        return entity;
    }

    public async Task DeleteAsync(TKey id, bool autoSave = false, CancellationToken cancellationToken = default) =>
        Items.Remove(await GetAsync(id, cancellationToken));

    // ReSharper disable once VirtualMemberNeverOverridden.Global
    protected virtual void Dispose(bool disposing)
    {
        // This method intentionally left blank.
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
