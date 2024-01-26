using ComplaintTracking.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ComplaintTracking.Controllers
{
    public partial class PublicController
    {
        private Task<PublicComplaintDetailsViewModel> GetPublicComplaintDetailsAsync(int complaintId) =>
            context.Complaints.AsNoTracking()
                .Include(e => e.ComplaintCounty)
                .Include(e => e.PrimaryConcern)
                .Include(e => e.SecondaryConcern)
                .Include(e => e.SourceState)
                .Include(e => e.CurrentOffice)
                .Where(e => !e.Deleted && e.ComplaintClosed)
                .Where(e => e.Id == complaintId)
                .Select(e => new PublicComplaintDetailsViewModel(e))
                .SingleOrDefaultAsync();

        private IQueryable<PublicComplaintActionViewModel> GetPublicComplaintActions(int complaintId) =>
            context.ComplaintActions.AsNoTracking()
                .Include(e => e.ActionType)
                .Where(e => e.ComplaintId == complaintId)
                .Where(e => !e.Deleted)
                .OrderBy(e => e.ActionDate)
                .Select(e => new PublicComplaintActionViewModel(e));

        public IQueryable<AttachmentViewModel> GetPublicComplaintAttachments(int complaintId) =>
            context.Attachments.AsNoTracking()
                .Where(e => e.ComplaintId == complaintId)
                .Where(e => !e.Deleted)
                .Where(e => !e.Complaint.Deleted && e.Complaint.ComplaintClosed)
                .Include(e => e.UploadedBy)
                .OrderBy(e => e.DateUploaded)
                .Select(e => new AttachmentViewModel(e));

        public Task<AttachmentViewModel> GetPublicAttachmentByIdAsync(Guid attachmentId) =>
            context.Attachments.AsNoTracking()
                .Where(e => e.Id == attachmentId)
                .Where(e => !e.Deleted)
                .Where(e => !e.Complaint.Deleted)
                .Where(e => e.Complaint.ComplaintClosed)
                .Select(e => new AttachmentViewModel(e))
                .SingleOrDefaultAsync();

        public Task<string> GetPublicAttachmentFilenameByIdAsync(Guid attachmentId) =>
            context.Attachments.AsNoTracking()
                .Where(e => e.Id == attachmentId)
                .Where(e => !e.Deleted)
                .Where(e => !e.Complaint.Deleted)
                .Where(e => e.Complaint.ComplaintClosed)
                .Select(e => e.FileName)
                .SingleOrDefaultAsync();
    }
}
