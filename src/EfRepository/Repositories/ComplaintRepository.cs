using Cts.Domain.Attachments;
using Cts.Domain.Complaints;
using Cts.EfRepository.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Cts.EfRepository.Repositories;

public class ComplaintRepository : BaseRepository<Complaint, int>, IComplaintRepository
{
    public ComplaintRepository(AppDbContext context) : base(context) { }

    public Task<bool> ExistsAsync(Expression<Func<Complaint, bool>> predicate, CancellationToken token = default) =>
        Context.Set<Complaint>().AsNoTracking().AnyAsync(predicate, token);

    public async Task<IReadOnlyCollection<Attachment>> GetAttachmentsListAsync(
        Expression<Func<Attachment, bool>> predicate, CancellationToken token = default) =>
        await Context.Set<Attachment>().AsNoTracking().Where(predicate).ToListAsync(token);

    public Task<Attachment?> FindAttachmentAsync(Guid id, CancellationToken token = default) =>
        Context.Set<Attachment>().AsNoTracking().SingleOrDefaultAsync(e => e.Id == id, cancellationToken: token);

    // Hide some base repository methods in order to include additional data.

    public new Task<Complaint?> FindAsync(
        Expression<Func<Complaint, bool>> predicate,
        CancellationToken token = default) =>
        Context.Set<Complaint>().AsNoTracking()
            .Include(e => e.ComplaintActions)
            .ThenInclude(e => e.ActionType)
            .SingleOrDefaultAsync(predicate, token);
}
