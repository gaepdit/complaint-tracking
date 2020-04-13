using ComplaintTracking.ViewModels;
using System.Linq;

namespace ComplaintTracking
{
    public partial class DAL
    {
        public IQueryable<ComplaintTransitionListViewModel> GetComplaintTransitions(int complaintId)
        {
            return _context.ComplaintTransitions
                .Where(e => e.Complaint.Id == complaintId)
                .OrderByDescending(e => e.DateTransferred)
                .Select(e => new ComplaintTransitionListViewModel
                {
                    TransferredByUser = e.TransferredByUser,
                    TransferredFromUser = e.TransferredFromUser,
                    TransferredFromOffice = e.TransferredFromOffice,
                    TransferredToUser = e.TransferredToUser,
                    TransferredToOffice = e.TransferredToOffice,
                    DateTransferred = e.DateTransferred,
                    DateAccepted = e.DateAccepted,
                    TransitionType = e.TransitionType,
                    Comment = e.Comment,
                });
        }
    }
}
