using Cts.Infrastructure.Contexts;
using GaEpd.Library.Domain.Entities;
using GaEpd.Library.Domain.Repositories;
using GaEpd.Library.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Cts.Infrastructure.Repositories;

public abstract class BaseReadOnlyRepository<TEntity, TKey> : IReadOnlyRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
    where TKey : IEquatable<TKey>
{
    protected readonly CtsDbContext DbContext;
    protected BaseReadOnlyRepository(CtsDbContext dbContext) => DbContext = dbContext;

    public async Task<TEntity> GetAsync(TKey id, CancellationToken token = default)
    {
        var item = await DbContext.Set<TEntity>().AsNoTracking()
            .SingleOrDefaultAsync(e => e.Id.Equals(id), token);
        return item ?? throw new EntityNotFoundException(typeof(TEntity), id);
    }

    public async Task<TEntity?> FindAsync(TKey id, CancellationToken token = default) =>
        throw new NotImplementedException();

    public async Task<TEntity?> FindAsync(Func<TEntity, bool> predicate, CancellationToken token = default) =>
        throw new NotImplementedException();

    public async Task<IList<TEntity>> GetListAsync(CancellationToken token = default) =>
        throw new NotImplementedException();

    public async Task<IList<TEntity>> GetListAsync(Func<TEntity, bool> predicate, CancellationToken token = default) =>
        throw new NotImplementedException();

    public async Task<IList<TEntity>> GetPagedListAsync(Func<TEntity, bool> predicate, PaginatedRequest paging,
        CancellationToken token = default) =>
        throw new NotImplementedException();

    public async Task<IList<TEntity>> GetPagedListAsync(PaginatedRequest paging, CancellationToken token = default) =>
        throw new NotImplementedException();

    // ReSharper disable once VirtualMemberNeverOverridden.Global
    protected virtual void Dispose(bool disposing)
    {
        if (disposing) DbContext.Dispose();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
