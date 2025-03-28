using Cts.Domain.Entities.ComplaintActions;
using Cts.TestData;
using GaEpd.AppLibrary.Pagination;
using System.Linq.Expressions;

namespace Cts.LocalRepository.Repositories;

public sealed class LocalActionRepository()
    : BaseRepository<ComplaintAction, Guid>(ComplaintActionData.GetComplaintActions), IActionRepository
{
    public Task<ComplaintAction?> FindIncludeAllAsync(Expression<Func<ComplaintAction, bool>> predicate,
        CancellationToken token = default) =>
        FindAsync(predicate, token);

    public Task<IReadOnlyCollection<ComplaintAction>> GetListAsync(Expression<Func<ComplaintAction, bool>> predicate,
        string ordering, string[] includeProperties, CancellationToken token = default) =>
        Task.FromResult<IReadOnlyCollection<ComplaintAction>>(Items.Where(predicate.Compile()).AsQueryable()
            .OrderByIf(ordering).ToList());
}
