using ComplaintTracking.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ComplaintTracking
{
    public partial class DAL
    {
        public IQueryable<AttachmentViewModel> GetAttachmentsByComplaintId(int complaintId)
        {
            return _context.Attachments.AsNoTracking()
                .Where(e => e.ComplaintId == complaintId)
                .Where(e => !e.Deleted)
                .Include(e => e.UploadedBy)
                .OrderBy(e => e.DateUploaded)
                .Select(e => new AttachmentViewModel(e));
        }

        public Task<AttachmentViewModel> GetAttachmentByIdAsync(Guid attachmentId)
        {
            return _context.Attachments.AsNoTracking()
                .Where(e => e.Id == attachmentId)
                .Where(e => !e.Deleted)
                .Where(e => !e.Complaint.Deleted)
                .Include(e => e.UploadedBy)
                .Select(e => new AttachmentViewModel(e))
                .SingleOrDefaultAsync();
        }

        public Task<string> GetAttachmentFilenameByIdAsync(Guid attachmentId)
        {
            return _context.Attachments.AsNoTracking()
                .Where(e => e.Id == attachmentId)
                .Where(e => !e.Deleted)
                .Where(e => !e.Complaint.Deleted)
                .Select(e => e.FileName)
                .SingleOrDefaultAsync();
        }

        public async Task<int> GetComplaintIdForAttachmentAsync(Guid attachmentId)
        {
            return await _context.Attachments.AsNoTracking()
                .Where(e => e.Id == attachmentId)
                .Where(e => !e.Deleted)
                .Select(e => e.ComplaintId)
                .SingleOrDefaultAsync();
        }
    }
}
