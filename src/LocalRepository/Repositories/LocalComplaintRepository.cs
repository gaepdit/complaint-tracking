using Cts.Domain.Attachments;
using Cts.Domain.ComplaintActions;
using Cts.Domain.Complaints;
using Cts.Domain.ComplaintTransitions;
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

    // Hide the base repository method in order to set the ID.

    public new Task InsertAsync(Complaint entity, bool autoSave = true, CancellationToken token = default)
    {
        entity.SetId(Items.Select(e => e.Id).Max() + 1);
        Items.Add(entity);
        return Task.CompletedTask;
    }
}
