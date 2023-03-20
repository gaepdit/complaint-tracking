using Cts.Domain.Attachments;
using Cts.Domain.ComplaintActions;
using Cts.Domain.ComplaintTransitions;
using System.Linq.Expressions;

namespace Cts.Domain.Complaints;

public interface IComplaintRepository : IRepository<Complaint, int>
{
    public Task<IReadOnlyCollection<ComplaintAction>> GetComplaintActionsListAsync(
        Expression<Func<ComplaintAction, bool>> predicate, CancellationToken token = default);

    public Task<IReadOnlyCollection<Attachment>> GetAttachmentsListAsync(
        Expression<Func<Attachment, bool>> predicate, CancellationToken token = default);

    public Task<IReadOnlyCollection<ComplaintTransition>> GetComplaintTransitionsListAsync(
        int complaintId, CancellationToken token = default);

    public Task<Attachment?> FindAttachmentAsync(Guid id, CancellationToken token = default);

    public Task<int> GetNextIdAsync(CancellationToken token = default);
}
