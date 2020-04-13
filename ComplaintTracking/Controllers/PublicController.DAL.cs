using ComplaintTracking.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ComplaintTracking.Controllers
{
    public partial class PublicController : Controller
    {
        private Task<PublicComplaintDetailsViewModel> GetPublicComplaintDetailsAsync(int complaintId)
        {
            return _context.Complaints.AsNoTracking()
                .Include(e => e.ComplaintCounty)
                .Include(e => e.PrimaryConcern)
                .Include(e => e.SecondaryConcern)
                .Include(e => e.SourceState)
                .Include(e => e.CurrentOffice)
                .Where(e => !e.Deleted && e.ComplaintClosed)
                .Where(e => e.Id == complaintId)
                .Select(e => new PublicComplaintDetailsViewModel(e))
                .SingleOrDefaultAsync();
        }

        private IQueryable<PublicComplaintActionViewModel> GetPublicComplaintActions(int complaintId)
        {
            return _context.ComplaintActions.AsNoTracking()
                .Include(e => e.ActionType)
                .Where(e => e.ComplaintId == complaintId)
                .Where(e => !e.Deleted)
                .OrderBy(e => e.ActionDate)
                .Select(e => new PublicComplaintActionViewModel(e));
        }

        public IQueryable<AttachmentViewModel> GetPublicComplaintAttachments(int complaintId)
        {
            return _context.Attachments.AsNoTracking()
                .Where(e => e.ComplaintId == complaintId)
                .Where(e => !e.Deleted)
                .Where(e => !e.Complaint.Deleted && e.Complaint.ComplaintClosed)
                .Include(e => e.UploadedBy)
                .OrderBy(e => e.DateUploaded)
                .Select(e => new AttachmentViewModel(e));
        }

        public async Task<AttachmentViewModel> GetAttachmentByIdAsync(Guid attachmentId)
        {
            return await _context.Attachments.AsNoTracking()
                .Where(e => e.Id == attachmentId)
                .Where(e => !e.Deleted)
                .Where(e => !e.Complaint.Deleted && e.Complaint.ComplaintClosed)
                .Include(e => e.UploadedBy)
                .Select(e => new AttachmentViewModel(e))
                .SingleOrDefaultAsync();
        }
    }
}
