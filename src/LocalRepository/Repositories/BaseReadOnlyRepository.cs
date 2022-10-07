using GaEpd.AppLibrary.Domain.Entities;
using GaEpd.AppLibrary.Domain.Repositories;
using GaEpd.AppLibrary.Pagination;
using System.Linq.Expressions;

namespace MyAppRoot.LocalRepository.Repositories;

public class BaseReadOnlyRepository<TEntity, TKey> : IReadOnlyRepository<TEntity, TKey>
    where TEntity : IEntity<TKey>
    where TKey : IEquatable<TKey>
{
    internal ICollection<TEntity> Items { get; }
    protected BaseReadOnlyRepository(IEnumerable<TEntity> items) => Items = items.ToList();

    public Task<TEntity> GetAsync(TKey id, CancellationToken token = default) =>
        Items.Any(e => e.Id.Equals(id))
            ? Task.FromResult(Items.Single(e => e.Id.Equals(id)))
            : throw new EntityNotFoundException(typeof(TEntity), id);

    public Task<TEntity?> FindAsync(TKey id, CancellationToken token = default) =>
        Task.FromResult(Items.SingleOrDefault(e => e.Id.Equals(id)));

    public Task<TEntity?> FindAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken token = default) =>
        Task.FromResult(Items.SingleOrDefault(predicate.Compile()));

    public Task<IReadOnlyCollection<TEntity>> GetListAsync(CancellationToken token = default) =>
        Task.FromResult(Items.ToList() as IReadOnlyCollection<TEntity>);

    public Task<IReadOnlyCollection<TEntity>> GetListAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken token = default) =>
        Task.FromResult(Items.Where(predicate.Compile()).ToList() as IReadOnlyCollection<TEntity>);

    public Task<IReadOnlyCollection<TEntity>> GetPagedListAsync(
        Expression<Func<TEntity, bool>> predicate,
        PaginatedRequest paging,
        CancellationToken token = default) =>
        Task.FromResult(Items.Where(predicate.Compile())
            .Skip(paging.Skip).Take(paging.Take).ToList() as IReadOnlyCollection<TEntity>);

    public Task<IReadOnlyCollection<TEntity>> GetPagedListAsync(
        PaginatedRequest paging,
        CancellationToken token = default) =>
        Task.FromResult(Items.Skip(paging.Skip).Take(paging.Take).ToList() as IReadOnlyCollection<TEntity>);

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
