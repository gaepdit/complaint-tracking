using Cts.Domain.Entities.Attachments;
using Cts.Domain.Entities.ComplaintActions;
using Cts.Domain.Entities.Complaints;
using Cts.Domain.Entities.ComplaintTransitions;
using Cts.TestData;
using System.Linq.Expressions;

namespace Cts.LocalRepository.Repositories;

public sealed class LocalComplaintRepository(IAttachmentRepository attachmentRepository,
        IComplaintActionRepository actionRepository, IComplaintTransitionRepository transitionRepository)
    : BaseRepository<Complaint, int>(ComplaintData.GetComplaints), IComplaintRepository
{
    public async Task<Complaint?> FindIncludeAllAsync(Expression<Func<Complaint, bool>> predicate,
        CancellationToken token = default)
    {
        var complaint = await FindAsync(predicate, token);
        if (complaint is null) return null;

        complaint.Attachments = (await attachmentRepository
                .GetListAsync(attachment => attachment.Complaint.Id == complaint.Id, token))
            .OrderByDescending(attachment => attachment.UploadedDate)
            .ToList();

        complaint.ComplaintActions = (await actionRepository
                .GetListAsync(action => action.Complaint.Id == complaint.Id && !action.IsDeleted, token))
            .OrderByDescending(action => action.ActionDate).ThenByDescending(action => action.EnteredDate)
            .ToList();

        complaint.ComplaintTransitions = (await transitionRepository
                .GetListAsync(transition => transition.Complaint.Id == complaint.Id, token))
            .OrderBy(transition => transition.CommittedDate)
            .ToList();

        return complaint;
    }

    public Task<Attachment?> FindAttachmentAsync(Guid id, CancellationToken token = default) =>
        attachmentRepository.FindAsync(id, token);

    public Task InsertTransitionAsync(ComplaintTransition transition, bool autoSave = true,
        CancellationToken token = default) =>
        transitionRepository.InsertAsync(transition, autoSave, token);

    // Local repository requires ID to be manually set.
    public int? GetNextId() => Items.Select(e => e.Id).Max() + 1;
}
