﻿using Cts.Domain.Entities.Attachments;
using Cts.Domain.Entities.ComplaintActions;
using Cts.Domain.Entities.ComplaintTransitions;
using System.Linq.Expressions;

namespace Cts.Domain.Entities.Complaints;

public interface IComplaintRepository : IRepository<Complaint, int>
{
    /// <summary>
    /// Returns the <see cref="Complaint"/> with the given <paramref name="id"/> and includes all additional
    /// properties (<see cref="ComplaintAction"/>, <see cref="Attachment"/>, & <see cref="ComplaintTransition"/>).
    /// Returns null if there are no matches.
    /// </summary>
    /// <param name="id">The Id of the Complaint.</param>
    /// <param name="includeDeletedActions">Whether to include deleted Complaint Actions in the result.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <exception cref="InvalidOperationException">Thrown if there are multiple matches.</exception>
    /// <returns>A Complaint entity.</returns>
    Task<Complaint?> FindIncludeAllAsync(int id, bool includeDeletedActions = false, CancellationToken token = default);

    /// <summary>
    /// Returns the <see cref="Complaint"/> matching the conditions of the <paramref name="predicate"/> and includes
    /// all additional properties (<see cref="ComplaintAction"/>, <see cref="Attachment"/>,
    /// & <see cref="ComplaintTransition"/>). Returns null if there are no matches.
    /// </summary>
    /// <param name="predicate">The search conditions.</param>
    /// <param name="includeDeletedActions">Whether to include deleted Complaint Actions in the result.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <exception cref="InvalidOperationException">Thrown if there are multiple matches.</exception>
    /// <returns>A Complaint entity.</returns>
    Task<Complaint?> FindIncludeAllAsync(Expression<Func<Complaint, bool>> predicate,
        bool includeDeletedActions = false, CancellationToken token = default);

    // Attachments

    /// <summary>
    /// Returns the <see cref="Attachment"/> matching the conditions of the <paramref name="predicate"/>.
    /// Returns null if no entity exists with the given Id.
    /// </summary>
    /// <param name="predicate">The search conditions.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>An Attachment or null.</returns>
    Task<Attachment?> FindAttachmentAsync(Expression<Func<Attachment, bool>> predicate,
        CancellationToken token = default);

    // Transitions

    /// <summary>
    /// Inserts a new <see cref="ComplaintTransition"/>.
    /// </summary>
    /// <param name="transition">The entity to insert.</param>
    /// <param name="autoSave">Whether to automatically save the changes (default is true).</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns><see cref="Task"/></returns>
    Task InsertTransitionAsync(ComplaintTransition transition, bool autoSave = true, CancellationToken token = default);

    // ID

    // Will return the next available ID if the repository requires it for adding new entities (local repository).
    // Will return null if the repository creates a new ID on insert (Entity Framework).
    int? GetNextId();
}
