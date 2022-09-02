using Cts.Infrastructure.Contexts;
using GaEpd.Library.Domain.Entities;
using GaEpd.Library.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Cts.Infrastructure.Repositories;

public abstract class BaseRepository<TEntity, TKey> : BaseReadOnlyRepository<TEntity, TKey>, IRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
    where TKey : IEquatable<TKey>
{
    protected BaseRepository(CtsDbContext dbContext) : base(dbContext) { }

    public async Task InsertAsync(TEntity entity, bool autoSave = false, CancellationToken token = default)
    {
        await DbContext.Set<TEntity>().AddAsync(entity, token);
        if (autoSave) await DbContext.SaveChangesAsync(token);
    }

    public async Task UpdateAsync(TEntity entity, bool autoSave = false, CancellationToken token = default)
    {
        DbContext.Attach(entity);
        DbContext.Update(entity);

        if (autoSave)
        {
            try
            {
                await DbContext.SaveChangesAsync(token);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await DbContext.Set<TEntity>().AsNoTracking().AnyAsync(e => e.Id.Equals(entity.Id), token))
                    throw new EntityNotFoundException(typeof(TEntity), entity.Id);

                throw;
            }
        }
    }

    public async Task DeleteAsync(TEntity entity, bool autoSave = false, CancellationToken token = default)
    {
        DbContext.Set<TEntity>().Remove(entity);
        if (autoSave) await DbContext.SaveChangesAsync(token);
    }
}
