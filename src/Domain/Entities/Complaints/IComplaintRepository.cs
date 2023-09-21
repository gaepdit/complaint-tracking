using Cts.Domain.Entities.Attachments;
using Cts.Domain.Entities.ComplaintActions;
using Cts.Domain.Entities.ComplaintTransitions;
using System.Linq.Expressions;

namespace Cts.Domain.Entities.Complaints;

public interface IComplaintRepository : IRepository<Complaint, int>
{
    // Actions

    /// <summary>
    /// Returns a read-only collection of <see cref="ComplaintAction"/> matching the conditions of the <paramref name="predicate"/>.
    /// Returns an empty collection if there are no matches.
    /// </summary>
    /// <param name="predicate">The search conditions.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>A read-only collection of entities.</returns>
    public Task<IReadOnlyCollection<ComplaintAction>> GetComplaintActionsListAsync(
        Expression<Func<ComplaintAction, bool>> predicate, CancellationToken token = default);

    // Attachments

    /// <summary>
    /// Returns a read-only collection of <see cref="Attachment"/> matching the conditions of the <paramref name="predicate"/>.
    /// Returns an empty collection if there are no matches.
    /// </summary>
    /// <param name="predicate">The search conditions.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>A read-only collection of entities.</returns>
    public Task<IReadOnlyCollection<Attachment>> GetAttachmentsListAsync(
        Expression<Func<Attachment, bool>> predicate, CancellationToken token = default);

    /// <summary>
    /// Returns the <see cref="Attachment"/> with the given <paramref name="id"/>.
    /// Returns null if no entity exists with the given Id.
    /// </summary>
    /// <param name="id">The Id of the entity.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>An Attachment or null.</returns>
    public Task<Attachment?> FindAttachmentAsync(Guid id, CancellationToken token = default);

    // Transitions

    /// <summary>
    /// Returns a read-only collection of <see cref="ComplaintTransition"/> for the
    /// <see cref="Complaint"/> with the given <paramref name="complaintId"/>.
    /// Returns an empty collection if there are no matches.
    /// </summary>
    /// <param name="complaintId">The Id of the complaint.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>A read-only collection of entities.</returns>
    public Task<IReadOnlyCollection<ComplaintTransition>> GetComplaintTransitionsListAsync(
        int complaintId, CancellationToken token = default);

    /// <summary>
    /// Inserts a new <see cref="ComplaintTransition"/>.
    /// </summary>
    /// <param name="transition">The entity to insert.</param>
    /// <param name="autoSave">Whether to automatically save the changes (default is true).</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns><see cref="Task"/></returns>
    Task InsertTransitionAsync(ComplaintTransition transition, bool autoSave = true, CancellationToken token = default);

    // Will return the next available ID if the repository requires it for adding new entities (local repository).
    // Will return null if the repository creates a new ID on insert (Entity Framework).
    public int? GetNextId();
}
