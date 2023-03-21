using Cts.Domain.Attachments;
using Cts.Domain.ComplaintActions;
using Cts.Domain.Complaints;
using Cts.Domain.ComplaintTransitions;
using Cts.EfRepository.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Cts.EfRepository.Repositories;

public class ComplaintRepository : BaseRepository<Complaint, int>, IComplaintRepository
{
    public ComplaintRepository(AppDbContext context) : base(context) { }

    public async Task<IReadOnlyCollection<ComplaintAction>> GetComplaintActionsListAsync(
        Expression<Func<ComplaintAction, bool>> predicate, CancellationToken token = default) =>
        await Context.ComplaintActions.AsNoTracking()
            .Where(predicate).ToListAsync(token);

    public async Task<IReadOnlyCollection<Attachment>> GetAttachmentsListAsync(
        Expression<Func<Attachment, bool>> predicate, CancellationToken token = default) =>
        await Context.Attachments.AsNoTracking()
            .Where(predicate).ToListAsync(token);

    public async Task<IReadOnlyCollection<ComplaintTransition>> GetComplaintTransitionsListAsync(
        int complaintId, CancellationToken token = default) =>
        await Context.ComplaintTransitions.AsNoTracking()
            .Where(e => e.Complaint.Id == complaintId).ToListAsync(token);

    public async Task<Attachment?> FindAttachmentAsync(Guid id, CancellationToken token = default) =>
        await Context.Attachments.AsNoTracking()
            .SingleOrDefaultAsync(e => e.Id == id, token);

    public async Task<int> GetNextIdAsync(CancellationToken token = default) =>
        await Context.Complaints.AsNoTracking().MaxAsync(e => e.Id, token) + 1;
}
