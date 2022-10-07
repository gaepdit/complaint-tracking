using GaEpd.AppLibrary.Domain.Entities;
using GaEpd.AppLibrary.Domain.Repositories;
using GaEpd.AppLibrary.Pagination;
using Microsoft.EntityFrameworkCore;
using MyAppRoot.Infrastructure.Contexts;
using System.Linq.Expressions;

namespace MyAppRoot.Infrastructure.Repositories;

public abstract class BaseReadOnlyRepository<TEntity, TKey> : IReadOnlyRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
    where TKey : IEquatable<TKey>
{
    protected readonly AppDbContext Context;
    protected BaseReadOnlyRepository(AppDbContext context) => Context = context;

    public async Task<TEntity> GetAsync(TKey id, CancellationToken token = default)
    {
        var item = await Context.Set<TEntity>().AsNoTracking()
            .SingleOrDefaultAsync(e => e.Id.Equals(id), token);
        return item ?? throw new EntityNotFoundException(typeof(TEntity), id);
    }

    public Task<TEntity?> FindAsync(TKey id, CancellationToken token = default) =>
        Context.Set<TEntity>().AsNoTracking().SingleOrDefaultAsync(e => e.Id.Equals(id), token);

    public Task<TEntity?> FindAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken token = default) =>
        Context.Set<TEntity>().AsNoTracking().SingleOrDefaultAsync(predicate, token);

    public async Task<IReadOnlyCollection<TEntity>> GetListAsync(CancellationToken token = default) =>
        await Context.Set<TEntity>().AsNoTracking().ToListAsync(token);

    public async Task<IReadOnlyCollection<TEntity>> GetListAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken token = default) =>
        await Context.Set<TEntity>().AsNoTracking().Where(predicate).ToListAsync(token);

    public async Task<IReadOnlyCollection<TEntity>> GetPagedListAsync(
        Expression<Func<TEntity, bool>> predicate,
        PaginatedRequest paging,
        CancellationToken token = default) =>
        await Context.Set<TEntity>().AsNoTracking().Where(predicate)
            .Skip(paging.Skip).Take(paging.Take).ToListAsync(token);

    public async Task<IReadOnlyCollection<TEntity>> GetPagedListAsync(
        PaginatedRequest paging,
        CancellationToken token = default) =>
        await Context.Set<TEntity>().AsNoTracking()
            .Skip(paging.Skip).Take(paging.Take).ToListAsync(token);


    // ReSharper disable once VirtualMemberNeverOverridden.Global
    protected virtual void Dispose(bool disposing)
    {
        if (disposing) Context.Dispose();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
