using Cts.Domain.Entities.Complaints;
using Cts.Domain.Entities.ComplaintTransitions;
using GaEpd.AppLibrary.Pagination;
using System.Linq.Expressions;

namespace Cts.EfRepository.Repositories;

public sealed class ComplaintRepository(AppDbContext context)
    : BaseRepository<Complaint, int, AppDbContext>(context), IComplaintRepository
{
    // EF will set the ID automatically.
    public int? GetNextId() => null;

    public async Task<Complaint?> FindIncludeAllAsync(int id, bool includeDeletedActions = false,
        CancellationToken token = default) =>
        await ComplaintIncludeAllQueryable(includeDeletedActions)
            .SingleOrDefaultAsync(complaint => complaint.Id.Equals(id), token).ConfigureAwait(false);

    public async Task<Complaint?> FindIncludeAllAsync(Expression<Func<Complaint, bool>> predicate,
        bool includeDeletedActions = false, CancellationToken token = default) =>
        await ComplaintIncludeAllQueryable(includeDeletedActions)
            .SingleOrDefaultAsync(predicate, token).ConfigureAwait(false);

    public async Task<IReadOnlyCollection<Complaint>> GetListWithMostRecentActionAsync(
        Expression<Func<Complaint, bool>> predicate, string sorting = "", CancellationToken token = default) =>
        await Context.Set<Complaint>().AsNoTracking().Where(predicate).OrderByIf(sorting)
            .Include(complaint => complaint.Actions
                .Where(action => !action.IsDeleted)
                .OrderByDescending(action => action.ActionDate)
                .ThenByDescending(action => action.EnteredDate)
                .ThenBy(action => action.Id)
                .Take(1)
            ).ThenInclude(action => action.ActionType)
            .ToListAsync(token).ConfigureAwait(false);

    private IQueryable<Complaint> ComplaintIncludeAllQueryable(
        bool includeDeletedActions) =>
        Context.Set<Complaint>()
            .Include(complaint => complaint.DeletedBy)
            .Include(complaint => complaint.Attachments
                .Where(attachment => !attachment.IsDeleted)
                .OrderByDescending(attachment => attachment.UploadedDate)
                .ThenBy(attachment => attachment.Id)
            )
            .Include(complaint => complaint.Actions
                .Where(action => !action.IsDeleted || includeDeletedActions)
                .OrderByDescending(action => action.ActionDate)
                .ThenByDescending(action => action.EnteredDate)
                .ThenBy(action => action.Id)
            ).ThenInclude(action => action.DeletedBy)
            .Include(complaint => complaint.ComplaintTransitions
                .OrderBy(transition => transition.CommittedDate)
                .ThenBy(transition => transition.Id)
            )
            .AsSplitQuery();

    public async Task InsertTransitionAsync(ComplaintTransition transition, bool autoSave = true,
        CancellationToken token = default)
    {
        await Context.Set<ComplaintTransition>().AddAsync(transition, token).ConfigureAwait(false);
        if (autoSave) await Context.SaveChangesAsync(token).ConfigureAwait(false);
    }
}
