using Cts.Domain.Entities.ComplaintActions;
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
}
