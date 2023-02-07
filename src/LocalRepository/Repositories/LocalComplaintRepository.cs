using Cts.Domain.Attachments;
using Cts.Domain.Complaints;
using Cts.TestData;
using System.Linq.Expressions;

namespace Cts.LocalRepository.Repositories;

public sealed class LocalComplaintRepository : BaseRepository<Complaint, int>, IComplaintRepository
{
    internal ICollection<Attachment> AttachmentItems { get; }

    public LocalComplaintRepository() : base(ComplaintData.GetComplaints)
    {
        AttachmentItems = AttachmentData.GetAttachments.ToList();
    }

    public Task<bool> ExistsAsync(Expression<Func<Complaint, bool>> predicate, CancellationToken token = default) =>
        Task.FromResult(Items.Any(predicate.Compile()));

    public Task<IReadOnlyCollection<Attachment>> GetAttachmentsListAsync(Expression<Func<Attachment, bool>> predicate,
        CancellationToken token = default) =>
        Task.FromResult(AttachmentItems.Where(predicate.Compile()).ToList() as IReadOnlyCollection<Attachment>);

    public Task<Attachment?> FindAttachmentAsync(Guid id, CancellationToken token = default) =>
        Task.FromResult(AttachmentItems.SingleOrDefault(e => e.Id == id));
}
