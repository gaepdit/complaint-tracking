using System;
using System.Linq;
using System.Threading.Tasks;
using ComplaintTracking.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ComplaintTracking.Controllers
{
    public partial class PublicController
    {
        private Task<PublicComplaintDetailsViewModel> GetPublicComplaintDetailsAsync(int complaintId) =>
            _context.Complaints.AsNoTracking()
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
            _context.ComplaintActions.AsNoTracking()
                .Include(e => e.ActionType)
                .Where(e => e.ComplaintId == complaintId)
                .Where(e => !e.Deleted)
                .OrderBy(e => e.ActionDate)
                .Select(e => new PublicComplaintActionViewModel(e));

        public IQueryable<AttachmentViewModel> GetPublicComplaintAttachments(int complaintId) =>
            _context.Attachments.AsNoTracking()
                .Where(e => e.ComplaintId == complaintId)
                .Where(e => !e.Deleted)
                .Where(e => !e.Complaint.Deleted && e.Complaint.ComplaintClosed)
                .Include(e => e.UploadedBy)
                .OrderBy(e => e.DateUploaded)
                .Select(e => new AttachmentViewModel(e));

        public Task<AttachmentViewModel> GetAttachmentByIdAsync(Guid attachmentId) =>
            _context.Attachments.AsNoTracking()
                .Where(e => e.Id == attachmentId)
                .Where(e => !e.Deleted)
                .Where(e => !e.Complaint.Deleted && e.Complaint.ComplaintClosed)
                .Include(e => e.UploadedBy)
                .Select(e => new AttachmentViewModel(e))
                .SingleOrDefaultAsync();
    }
}
