using Cts.Domain.Entities.Attachments;
using Cts.Domain.Entities.ComplaintActions;
using Cts.Domain.Entities.Complaints;
using Cts.Domain.Entities.ComplaintTransitions;
using Cts.TestData;
using System.Linq.Expressions;

namespace Cts.LocalRepository.Repositories;

public sealed class LocalComplaintRepository : BaseRepository<Complaint, int>, IComplaintRepository
{
    internal ICollection<ComplaintAction> ComplaintActionItems { get; }
    internal ICollection<Attachment> AttachmentItems { get; }
    internal ICollection<ComplaintTransition> ComplaintTransitionItems { get; }

    public LocalComplaintRepository() : base(ComplaintData.GetComplaints)
    {
        ComplaintActionItems = ComplaintActionData.GetComplaintActions.ToList();
        AttachmentItems = AttachmentData.GetAttachments.ToList();
        ComplaintTransitionItems = ComplaintTransitionData.GetComplaintTransitions.ToList();
    }

    public Task<IReadOnlyCollection<ComplaintAction>> GetComplaintActionsListAsync(
        Expression<Func<ComplaintAction, bool>> predicate, CancellationToken token = default) =>
        Task.FromResult(ComplaintActionItems.Where(predicate.Compile()).ToList()
            as IReadOnlyCollection<ComplaintAction>);

    public Task<IReadOnlyCollection<Attachment>> GetAttachmentsListAsync(
        Expression<Func<Attachment, bool>> predicate, CancellationToken token = default) =>
        Task.FromResult(AttachmentItems.Where(predicate.Compile()).ToList() as IReadOnlyCollection<Attachment>);

    public Task<IReadOnlyCollection<ComplaintTransition>> GetComplaintTransitionsListAsync(
        int complaintId, CancellationToken token = default) =>
        Task.FromResult(ComplaintTransitionItems.Where(e => e.Complaint.Id == complaintId)
            .ToList() as IReadOnlyCollection<ComplaintTransition>);

    public Task<Attachment?> FindAttachmentAsync(Guid id, CancellationToken token = default) =>
        Task.FromResult(AttachmentItems.SingleOrDefault(e => e.Id == id));

    public Task<int> GetNextIdAsync(CancellationToken token = default) =>
        Task.FromResult(Items.Select(e => e.Id).Max() + 1);
}
