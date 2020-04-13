using System;
using ComplaintTracking.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ComplaintTracking.Data
{
    public partial class SeedTestData
    {
        public static async Task<ComplaintTransition[]> GetComplaintTransitions(
            ApplicationDbContext _context,
            ApplicationUser user
            )
        {
            var office = await _context.LookupOffices.FirstOrDefaultAsync(e => e.Name == "Director's Office");

            ComplaintTransition[] c = {
                new ComplaintTransition {
                    Complaint = await _context.Complaints.FirstOrDefaultAsync(e => e.Status == ComplaintStatus.Closed),
                    DateAccepted = DateTime.Parse("Jul 1, 2017"),
                    DateTransferred = DateTime.Parse("Jul 1, 2017"),
                    Comment = "Assigned",
                    TransferredByUser = user,
                    TransferredFromUser = user,
                    TransferredFromOffice = office,
                    TransferredToOffice = office,
                    TransferredToUser = user,
                    TransitionType = TransitionType.Assigned
                },
                new ComplaintTransition {
                    Complaint = await _context.Complaints.FirstOrDefaultAsync(e => e.Status == ComplaintStatus.Closed),
                    DateAccepted = DateTime.Parse("Jul 2, 2017"),
                    DateTransferred = DateTime.Parse("Jul 2, 2017"),
                    Comment = "Submitted for review",
                    TransferredByUser = user,
                    TransferredFromUser = user,
                    TransferredFromOffice = office,
                    TransferredToOffice = office,
                    TransferredToUser = user,
                    TransitionType = TransitionType.SubmittedForReview
                },
                new ComplaintTransition {
                    Complaint = await _context.Complaints.FirstOrDefaultAsync(e => e.Status == ComplaintStatus.Closed),
                    DateAccepted = DateTime.Parse("Jul 6, 2017"),
                    DateTransferred = DateTime.Parse("Jul 6, 2017"),
                    Comment = "Closed",
                    TransferredByUser = user,
                    TransferredFromUser = user,
                    TransferredFromOffice = office,
                    TransferredToOffice = office,
                    TransferredToUser = user,
                    TransitionType = TransitionType.Closed
                },
                new ComplaintTransition {
                    Complaint = await _context.Complaints.FirstOrDefaultAsync(e => e.Deleted == true),
                    DateAccepted = DateTime.Parse("Jul 5, 2017"),
                    DateTransferred = DateTime.Parse("Jul 5, 2017"),
                    Comment = "Assigned",
                    TransferredByUser = user,
                    TransferredFromUser = user,
                    TransferredFromOffice = office,
                    TransferredToOffice = office,
                    TransferredToUser = user,
                    TransitionType = TransitionType.Assigned
                },
                new ComplaintTransition {
                    Complaint = await _context.Complaints.FirstOrDefaultAsync(e => e.Deleted == true),
                    DateAccepted = DateTime.Parse("Jul 6, 2017"),
                    DateTransferred = DateTime.Parse("Jul 6, 2017"),
                    Comment = "Submitted for review",
                    TransferredByUser = user,
                    TransferredFromUser = user,
                    TransferredFromOffice = office,
                    TransferredToOffice = office,
                    TransferredToUser = user,
                    TransitionType = TransitionType.SubmittedForReview
                },
                new ComplaintTransition {
                    Complaint = await _context.Complaints.FirstOrDefaultAsync(e => e.Deleted == true),
                    DateAccepted = DateTime.Parse("Jul 16, 2017"),
                    DateTransferred = DateTime.Parse("Jul 16, 2017"),
                    Comment = "Deleted",
                    TransferredByUser = user,
                    TransferredFromUser = user,
                    TransferredFromOffice = office,
                    TransferredToOffice = office,
                    TransferredToUser = user,
                    TransitionType = TransitionType.Deleted
                },
                 new ComplaintTransition {
                    Complaint = await _context.Complaints.FirstOrDefaultAsync(e => e.Status == ComplaintStatus.ReviewPending && e.Deleted == false),
                    DateAccepted = DateTime.Parse("Jul 8, 2017"),
                    DateTransferred = DateTime.Parse("Jul 6, 2017"),
                    Comment = "Assigned",
                    TransferredByUser = user,
                    TransferredFromUser = user,
                    TransferredFromOffice = office,
                    TransferredToOffice = office,
                    TransferredToUser = user,
                    TransitionType = TransitionType.Assigned
                },
                new ComplaintTransition {
                    Complaint = await _context.Complaints.FirstOrDefaultAsync(e => e.Status == ComplaintStatus.ReviewPending && e.Deleted == false),
                    DateAccepted = DateTime.Parse("Jul 11, 2017"),
                    DateTransferred = DateTime.Parse("Jul 11, 2017"),
                    Comment = "Submitted for review",
                    TransferredByUser = user,
                    TransferredFromUser = user,
                    TransferredFromOffice = office,
                    TransferredToOffice = office,
                    TransferredToUser = user,
                    TransitionType = TransitionType.SubmittedForReview
                },
                 new ComplaintTransition {
                    Complaint = await _context.Complaints.FirstOrDefaultAsync(e => e.Status == ComplaintStatus.UnderInvestigation),
                    DateAccepted = DateTime.Parse("Jul 13, 2017"),
                    DateTransferred = DateTime.Parse("Jul 13, 2017"),
                    Comment = "Assigned",
                    TransferredByUser = user,
                    TransferredFromUser = user,
                    TransferredFromOffice = office,
                    TransferredToOffice = office,
                    TransferredToUser = user,
                    TransitionType = TransitionType.Assigned
                },
           };

            return c;
        }
    }
}