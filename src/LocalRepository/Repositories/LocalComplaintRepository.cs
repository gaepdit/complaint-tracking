using Cts.Domain.Entities.Complaints;
using Cts.Domain.Entities.ComplaintTransitions;
using Cts.TestData;
using GaEpd.AppLibrary.Pagination;
using System.Linq.Expressions;

namespace Cts.LocalRepository.Repositories;

public sealed class LocalComplaintRepository(IComplaintTransitionRepository transitionRepository)
    : BaseRepository<Complaint, int>(ComplaintData.GetComplaints), IComplaintRepository
{
    // Local repository requires ID to be manually set.
    public int? GetNextId() => Items.Select(e => e.Id).Max() + 1;

    public async Task<Complaint?> FindIncludeAllAsync(int id, bool includeDeletedActions = false,
        CancellationToken token = default) =>
        CleanComplaintActions(await FindAsync(id, token).ConfigureAwait(false), includeDeletedActions);

    public async Task<Complaint?> FindPublicAsync(Expression<Func<Complaint, bool>> predicate,
        CancellationToken token = default) =>
        CleanComplaintActions(await FindAsync(predicate, token).ConfigureAwait(false), false);

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

        return Task.FromResult(complaints.ToList() as IReadOnlyCollection<Complaint>);
    }

    private static Complaint? CleanComplaintActions(Complaint? complaint, bool includeDeletedActions)
    {
        if (complaint is null) return null;
        complaint.Attachments.RemoveAll(attachment => attachment.IsDeleted);
        if (!includeDeletedActions) complaint.Actions.RemoveAll(action => action.IsDeleted);
        return complaint;
    }

    public Task InsertTransitionAsync(ComplaintTransition transition, bool autoSave = true,
        CancellationToken token = default) =>
        transitionRepository.InsertAsync(transition, autoSave, token);
}
