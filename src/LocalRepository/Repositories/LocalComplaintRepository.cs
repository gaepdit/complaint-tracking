using Cts.Domain.Entities.Attachments;
using Cts.Domain.Entities.ComplaintActions;
using Cts.Domain.Entities.Complaints;
using Cts.Domain.Entities.ComplaintTransitions;
using Cts.TestData;
using GaEpd.AppLibrary.Pagination;
using System.Linq.Expressions;

namespace Cts.LocalRepository.Repositories;

public sealed class LocalComplaintRepository(
    IAttachmentRepository attachmentRepository,
    IActionRepository actionRepository,
    IComplaintTransitionRepository transitionRepository)
    : BaseRepository<Complaint, int>(ComplaintData.GetComplaints), IComplaintRepository
{
    // Local repository requires ID to be manually set.
    public int? GetNextId() => Items.Select(e => e.Id).Max() + 1;

    public async Task<Complaint?> FindIncludeAllAsync(int id, bool includeDeletedActions = false,
        CancellationToken token = default) =>
        await GetComplaintDetailsAsync(await FindAsync(id, token: token).ConfigureAwait(false), includeDeletedActions, token: token)
            .ConfigureAwait(false);

    public async Task<Complaint?> FindPublicAsync(Expression<Func<Complaint, bool>> predicate,
        CancellationToken token = default) =>
        await GetComplaintDetailsAsync(await FindAsync(predicate, token: token).ConfigureAwait(false), false, token: token)
            .ConfigureAwait(false);

    public Task<IReadOnlyCollection<Complaint>> GetListWithMostRecentActionAsync(
        Expression<Func<Complaint, bool>> predicate, string sorting = "", CancellationToken token = default)
    {
        var complaints = Items.Where(predicate.Compile()).AsQueryable().OrderByIf(sorting);

#pragma warning disable S3267 // Loops should be simplified with "LINQ" expressions
        foreach (var complaint in complaints.Where(complaint => complaint.Actions.Count > 0))
        {
            complaint.Actions.RemoveAll(action => action.IsDeleted);
            if (complaint.Actions.Count > 1)
                complaint.Actions.RemoveRange(0, complaint.Actions.Count - 1);
        }
#pragma warning restore S3267

        return Task.FromResult<IReadOnlyCollection<Complaint>>(complaints.ToList());
    }

    private async Task<Complaint?> GetComplaintDetailsAsync(Complaint? complaint, bool includeDeletedActions,
        CancellationToken token)
    {
        if (complaint is null) return null;

        complaint.Attachments.Clear();
        complaint.Attachments.AddRange((await attachmentRepository.GetListAsync(attachment =>
                    attachment.Complaint.Id == complaint.Id && !attachment.IsDeleted, token: token)
                .ConfigureAwait(false))
            .OrderBy(attachment => attachment.UploadedDate)
            .ThenBy(attachment => attachment.FileName)
            .ThenBy(attachment => attachment.Id));

        complaint.Actions.Clear();
        complaint.Actions.AddRange((await actionRepository.GetListAsync(action =>
                    action.Complaint.Id == complaint.Id && (!action.IsDeleted || includeDeletedActions), token: token)
                .ConfigureAwait(false))
            .OrderByDescending(action => action.ActionDate)
            .ThenByDescending(action => action.EnteredDate)
            .ThenBy(action => action.Id));

        complaint.ComplaintTransitions.Clear();
        complaint.ComplaintTransitions.AddRange((await transitionRepository.GetListAsync(transition =>
                    transition.Complaint.Id == complaint.Id, token: token)
                .ConfigureAwait(false))
            .OrderBy(transition => transition.CommittedDate).ThenBy(transition => transition.Id));

        return complaint;
    }

    public Task InsertTransitionAsync(ComplaintTransition transition, bool autoSave = true,
        CancellationToken token = default) =>
        transitionRepository.InsertAsync(transition, autoSave, token: token);
}
