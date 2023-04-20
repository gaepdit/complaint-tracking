using Cts.Domain.Entities.Attachments;
using Cts.Domain.Entities.ComplaintActions;
using Cts.Domain.Entities.ComplaintTransitions;
using System.Linq.Expressions;

namespace Cts.Domain.Entities.Complaints;

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
}
