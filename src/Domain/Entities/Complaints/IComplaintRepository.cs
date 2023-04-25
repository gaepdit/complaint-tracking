using Cts.Domain.Entities.Attachments;
using Cts.Domain.Entities.ComplaintActions;
using Cts.Domain.Entities.ComplaintTransitions;
using System.Linq.Expressions;

namespace Cts.Domain.Entities.Complaints;

/// <inheritdoc />
public interface IComplaintRepository : IRepository<Complaint, int>
{
    public Task<IReadOnlyCollection<ComplaintAction>> GetComplaintActionsListAsync(
        Expression<Func<ComplaintAction, bool>> predicate, CancellationToken token = default);

    public Task<IReadOnlyCollection<Attachment>> GetAttachmentsListAsync(
        Expression<Func<Attachment, bool>> predicate, CancellationToken token = default);

    public Task<IReadOnlyCollection<ComplaintTransition>> GetComplaintTransitionsListAsync(
        int complaintId, CancellationToken token = default);

    public Task<Attachment?> FindAttachmentAsync(Guid id, CancellationToken token = default);

    // Will return the next available ID if the repository requires it for adding new entities (local repository).
    // Will return null if the repository creates a new ID on insert (Entity Framework).
    public Task<int?> GetNextIdAsync();

    // TODO: Move this to GaEpd.AppLibrary.Domain.Repositories
    /// <summary>
    /// Saves all changes to the repository. Only needed by repositories that require explicit calls to save changes.
    /// Used with <see cref="IWriteRepository{TEntity,TKey}"/> when the autoSave parameter is set to false.
    /// </summary>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns></returns>
    Task SaveChangesAsync(CancellationToken token = default);
}
