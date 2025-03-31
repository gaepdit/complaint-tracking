using Cts.Domain.Entities.Complaints;
using System.Linq.Expressions;

namespace Cts.Domain.Entities.ComplaintActions;

public interface IActionRepository : IRepository<ComplaintAction, Guid>
{
    /// <summary>
    /// Returns the <see cref="ComplaintAction"/> matching the conditions of the <paramref name="predicate"/>
    /// and including additional properties (<see cref="Complaint"/>). Returns null if there are no matches.
    /// </summary>
    /// <param name="predicate">The search conditions.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <exception cref="InvalidOperationException">Thrown if there are multiple matches.</exception>
    /// <returns>A Complaint Action entity.</returns>
    Task<ComplaintAction?> FindIncludeAllAsync(Expression<Func<ComplaintAction, bool>> predicate,
        CancellationToken token = default);
}
