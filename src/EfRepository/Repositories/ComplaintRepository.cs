using Cts.Domain.Entities.Attachments;
using Cts.Domain.Entities.ComplaintActions;
using Cts.Domain.Entities.Complaints;
using Cts.Domain.Entities.ComplaintTransitions;
using Cts.EfRepository.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Cts.EfRepository.Repositories;

/// <inheritdoc cref="IComplaintRepository" />
public class ComplaintRepository : BaseRepository<Complaint, int>, IComplaintRepository
{
    public ComplaintRepository(AppDbContext context) : base(context) { }

    public async Task<IReadOnlyCollection<ComplaintAction>> GetComplaintActionsListAsync(
        Expression<Func<ComplaintAction, bool>> predicate, CancellationToken token = default) =>
        await Context.ComplaintActions.AsNoTracking()
            .Where(predicate)
            .OrderBy(e => e.ActionDate)
            .ToListAsync(token);

    public async Task<IReadOnlyCollection<Attachment>> GetAttachmentsListAsync(
        Expression<Func<Attachment, bool>> predicate, CancellationToken token = default) =>
        await Context.Attachments.AsNoTracking()
            .Where(predicate)
            .OrderBy(e => e.UploadedDate)
            .ToListAsync(token);

    public async Task<IReadOnlyCollection<ComplaintTransition>> GetComplaintTransitionsListAsync(
        int complaintId, CancellationToken token = default) =>
        await Context.ComplaintTransitions.AsNoTracking()
            .Where(e => e.Complaint.Id == complaintId)
            .OrderBy(e => e.CommittedDate)
            .ToListAsync(token);

    public async Task InsertTransitionAsync(
        ComplaintTransition transition, bool autoSave = true, CancellationToken token = default)
    {
        await Context.Set<ComplaintTransition>().AddAsync(transition, token);
        if (autoSave) await Context.SaveChangesAsync(token);
    }

    public async Task<Attachment?> FindAttachmentAsync(Guid id, CancellationToken token = default) =>
        await Context.Attachments
            .Include(e => e.Complaint)
            .SingleOrDefaultAsync(e => e.Id == id, token);

    // EF will set the ID automatically.
    public Task<int?> GetNextIdAsync() => Task.FromResult(null as int?);
}
