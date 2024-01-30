using Cts.Domain.Entities.Attachments;
using Cts.Domain.Entities.Complaints;
using Cts.Domain.Entities.ComplaintTransitions;
using System.Linq.Expressions;

namespace Cts.EfRepository.Repositories;

public sealed class ComplaintRepository(AppDbContext context)
    : BaseRepository<Complaint, int, AppDbContext>(context), IComplaintRepository
{
    public async Task<Complaint?> FindIncludeAllAsync(int id, bool includeDeletedActions = false,
        CancellationToken token = default) =>
        await Context.Set<Complaint>()
            .Include(complaint => complaint.Attachments
                .Where(attachment => !attachment.IsDeleted)
                .OrderByDescending(attachment => attachment.UploadedDate)
            )
            .Include(complaint => complaint.ComplaintActions
                .Where(action => !action.IsDeleted || includeDeletedActions)
                .OrderByDescending(action => action.ActionDate)
                .ThenByDescending(action => action.EnteredDate)
            )
            .Include(complaint => complaint.ComplaintTransitions
                .OrderBy(transition => transition.CommittedDate))
            .SingleOrDefaultAsync(e => e.Id.Equals(id), token);


    public async Task<Attachment?> FindAttachmentAsync(Guid id, CancellationToken token = default) =>
        await Context.Set<Attachment>()
            .Include(e => e.Complaint)
            .SingleOrDefaultAsync(e => e.Id == id, token);

    public async Task InsertTransitionAsync(ComplaintTransition transition, bool autoSave = true,
        CancellationToken token = default)
    {
        await Context.Set<ComplaintTransition>().AddAsync(transition, token);
        if (autoSave) await Context.SaveChangesAsync(token);
    }

    // EF will set the ID automatically.
    public int? GetNextId() => null;
}
