using Cts.Domain.Entities.ComplaintActions;
using GaEpd.AppLibrary.Pagination;
using System.Linq.Expressions;

namespace Cts.EfRepository.Repositories;

public sealed class ActionRepository(AppDbContext context)
    : BaseRepository<ComplaintAction, Guid, AppDbContext>(context), IActionRepository
{
    public Task<ComplaintAction?> FindIncludeAllAsync(Expression<Func<ComplaintAction, bool>> predicate,
        CancellationToken token = default) =>
        Context.ComplaintActions.AsNoTracking()
            .Include(action => action.Complaint)
            .SingleOrDefaultAsync(predicate, token);

    public async Task<IReadOnlyCollection<ComplaintAction>> GetListAsync(
        Expression<Func<ComplaintAction, bool>> predicate, string ordering, CancellationToken token = default) =>
        await Context.ComplaintActions.AsNoTracking().Where(predicate).OrderByIf(ordering)
            .Include(action => action.Complaint).ToListAsync(token).ConfigureAwait(false);

    public async Task<IReadOnlyCollection<ComplaintAction>> GetPagedListAsync(
        Expression<Func<ComplaintAction, bool>> predicate, PaginatedRequest paging, string[] includeProperties,
        CancellationToken token = default) =>
        await includeProperties.Aggregate(Context.Set<ComplaintAction>().AsNoTracking(),
                (queryable, includeProperty) => queryable.Include(includeProperty))
            .Where(predicate).OrderByIf(paging.Sorting)
            .Skip(paging.Skip).Take(paging.Take).ToListAsync(token).ConfigureAwait(false);
}
