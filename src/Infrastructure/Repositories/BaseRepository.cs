using GaEpd.AppLibrary.Domain.Entities;
using GaEpd.AppLibrary.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using MyAppRoot.Infrastructure.Contexts;

namespace MyAppRoot.Infrastructure.Repositories;

public abstract class BaseRepository<TEntity, TKey> : BaseReadOnlyRepository<TEntity, TKey>, IRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
    where TKey : IEquatable<TKey>
{
    protected BaseRepository(AppDbContext context) : base(context) { }

    public async Task InsertAsync(TEntity entity, bool autoSave = true, CancellationToken token = default)
    {
        await Context.Set<TEntity>().AddAsync(entity, token);
        if (autoSave) await Context.SaveChangesAsync(token);
    }

    public async Task UpdateAsync(TEntity entity, bool autoSave = true, CancellationToken token = default)
    {
        Context.Attach(entity);
        Context.Update(entity);

        if (!autoSave) return;

        try
        {
            await Context.SaveChangesAsync(token);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await Context.Set<TEntity>().AsNoTracking().AnyAsync(e => e.Id.Equals(entity.Id), token))
                throw new EntityNotFoundException(typeof(TEntity), entity.Id);
            throw;
        }
    }

    public async Task DeleteAsync(TEntity entity, bool autoSave = true, CancellationToken token = default)
    {
        Context.Set<TEntity>().Remove(entity);

        try
        {
            if (autoSave) await Context.SaveChangesAsync(token);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await Context.Set<TEntity>().AsNoTracking().AnyAsync(e => e.Id.Equals(entity.Id), token))
                throw new EntityNotFoundException(typeof(TEntity), entity.Id);
            throw;
        }
    }
}
