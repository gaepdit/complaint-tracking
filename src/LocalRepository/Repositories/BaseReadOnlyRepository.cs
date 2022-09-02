﻿using GaEpd.Library.Domain.Entities;
using GaEpd.Library.Domain.Repositories;
using GaEpd.Library.Pagination;

namespace Cts.LocalRepository.Repositories;

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

    public Task<TEntity?> FindAsync(Func<TEntity, bool> predicate,
        CancellationToken token = default) =>
        Task.FromResult(Items.SingleOrDefault(predicate));

    public Task<IList<TEntity>> GetListAsync(CancellationToken token = default) =>
        Task.FromResult(Items.ToList() as IList<TEntity>);

    public Task<IList<TEntity>> GetListAsync(Func<TEntity, bool> predicate, CancellationToken token = default) =>
        Task.FromResult(Items.Where(predicate).ToList() as IList<TEntity>);

    public Task<IList<TEntity>> GetPagedListAsync(PaginatedRequest paging, CancellationToken token = default) =>
        Task.FromResult(Items.Skip(paging.Skip).Take(paging.Take).ToList() as IList<TEntity>);

    public Task<IList<TEntity>> GetPagedListAsync(
        Func<TEntity, bool> predicate,
        PaginatedRequest paging,
        CancellationToken token = default) =>
        Task.FromResult(Items.Where(predicate).Skip(paging.Skip).Take(paging.Take).ToList() as IList<TEntity>);

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