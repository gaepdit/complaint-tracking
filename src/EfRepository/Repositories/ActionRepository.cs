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
        Expression<Func<ComplaintAction, bool>> predicate, string ordering, string[] includeProperties,
        CancellationToken token = default) =>
        await includeProperties.Aggregate(Context.Set<ComplaintAction>().AsNoTracking(),
                (queryable, includeProperty) => queryable.Include(includeProperty))
            .Where(predicate).OrderByIf(ordering).ToListAsync(token)
            .ConfigureAwait(false);
}
