using Cts.Domain.Entities.ComplaintActions;
using Cts.TestData;
using System.Linq.Expressions;

namespace Cts.LocalRepository.Repositories;

public sealed class LocalActionRepository()
    : BaseRepository<ComplaintAction, Guid>(ComplaintActionData.GetComplaintActions), IActionRepository
{
    public Task<ComplaintAction?> FindIncludeAllAsync(Expression<Func<ComplaintAction, bool>> predicate,
        CancellationToken token = default) =>
        FindAsync(predicate, token: token);
}
