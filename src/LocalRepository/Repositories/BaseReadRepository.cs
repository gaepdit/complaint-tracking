using GaEpd.AppLibrary.Domain.Entities;
using GaEpd.AppLibrary.Domain.Repositories;
using GaEpd.AppLibrary.Pagination;
using System.Linq.Expressions;

namespace Cts.LocalRepository.Repositories;

public abstract class BaseReadRepository<TEntity, TKey> : IReadRepository<TEntity, TKey>
    where TEntity : IEntity<TKey>
    where TKey : IEquatable<TKey>
{
    internal ICollection<TEntity> Items { get; }

    protected BaseReadRepository(IEnumerable<TEntity> items)
    {
        Items = items.ToList();
    }

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
        CancellationToken token = default)
    {
        var result = Items.Where(predicate.Compile()).AsQueryable()
            .OrderByIf(paging.Sorting)
            .Skip(paging.Skip).Take(paging.Take);
        return Task.FromResult(result.ToList() as IReadOnlyCollection<TEntity>);
    }

    public Task<IReadOnlyCollection<TEntity>> GetPagedListAsync(
        PaginatedRequest paging,
        CancellationToken token = default)
    {
        var result = Items.AsQueryable()
            .OrderByIf(paging.Sorting)
            .Skip(paging.Skip).Take(paging.Take);
        return Task.FromResult(result.ToList() as IReadOnlyCollection<TEntity>);
    }

    public Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default) =>
        Task.FromResult(Items.Count(predicate.Compile()));

    public Task<bool> ExistsAsync(TKey id, CancellationToken token = default) =>
        Task.FromResult(Items.Any(e => e.Id.Equals(id)));

    public Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default) =>
        Task.FromResult(Items.Any(predicate.Compile()));

    // ReSharper disable once VirtualMemberNeverOverridden.Global
    // ReSharper disable once UnusedParameter.Global
    protected virtual void Dispose(bool disposing) { }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
