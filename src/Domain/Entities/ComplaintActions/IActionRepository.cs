using Cts.Domain.Entities.Complaints;
using GaEpd.AppLibrary.Pagination;
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

    // FUTURE: These may be included in a future App Library update.

    /// <summary>
    /// Returns a read-only collection of <see cref="ComplaintAction"/> matching the conditions of the
    /// <paramref name="predicate"/>. Returns an empty collection if there are no matches.
    /// </summary>
    /// <param name="predicate">The search conditions.</param>
    /// <param name="ordering">An expression string to indicate values to order by.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>A read-only collection of entities.</returns>
    Task<IReadOnlyCollection<ComplaintAction>> GetListAsync(Expression<Func<ComplaintAction, bool>> predicate,
        string ordering, CancellationToken token = default);

    /// <summary>
    /// Returns a filtered, read-only collection of <see cref="ComplaintAction"/> matching the conditions of the
    /// <paramref name="predicate"/>. Returns an empty collection if there are no matches.
    /// </summary>
    /// <param name="predicate">The search conditions.</param>
    /// <param name="paging">A <see cref="PaginatedRequest"/> to define the paging options.</param>
    /// <param name="includeProperties">Navigation properties to include (when using an Entity Framework repository).</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>A sorted and paged read-only collection of entities.</returns>
    Task<IReadOnlyCollection<ComplaintAction>> GetPagedListAsync(Expression<Func<ComplaintAction, bool>> predicate,
        PaginatedRequest paging, string[] includeProperties, CancellationToken token = default);
}
