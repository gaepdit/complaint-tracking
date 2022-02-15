using System;
using System.Threading.Tasks;
using ComplaintTracking.Models;
using Microsoft.EntityFrameworkCore;

namespace ComplaintTracking.Data
{
    public static partial class SeedTestData
    {
        public static async Task<ComplaintTransition[]> GetComplaintTransitions(
            ApplicationDbContext context, ApplicationUser user)
        {
            var office = await context.LookupOffices.FirstOrDefaultAsync(e => e.Name == "Director's Office");

            ComplaintTransition[] c =
            {
                new ComplaintTransition
                {
                    Complaint = await context.Complaints.FirstOrDefaultAsync(e => e.Status == ComplaintStatus.Closed),
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
                new ComplaintTransition
                {
                    Complaint = await context.Complaints.FirstOrDefaultAsync(e => e.Status == ComplaintStatus.Closed),
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
                new ComplaintTransition
                {
                    Complaint = await context.Complaints.FirstOrDefaultAsync(e => e.Status == ComplaintStatus.Closed),
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
                new ComplaintTransition
                {
                    Complaint = await context.Complaints.FirstOrDefaultAsync(e => e.Deleted),
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
                new ComplaintTransition
                {
                    Complaint = await context.Complaints.FirstOrDefaultAsync(e => e.Deleted),
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
                new ComplaintTransition
                {
                    Complaint = await context.Complaints.FirstOrDefaultAsync(e => e.Deleted),
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
                new ComplaintTransition
                {
                    Complaint = await context.Complaints.FirstOrDefaultAsync(e =>
                        e.Status == ComplaintStatus.ReviewPending && !e.Deleted),
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
                new ComplaintTransition
                {
                    Complaint = await context.Complaints.FirstOrDefaultAsync(e =>
                        e.Status == ComplaintStatus.ReviewPending && !e.Deleted),
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
                new ComplaintTransition
                {
                    Complaint = await context.Complaints.FirstOrDefaultAsync(e =>
                        e.Status == ComplaintStatus.UnderInvestigation),
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
                new ComplaintTransition
                {
                    Complaint = await context.Complaints.FirstOrDefaultAsync(e => e.Status == ComplaintStatus.AdministrativelyClosed),
                    DateAccepted = DateTime.Parse("Jul 1, 2015"),
                    DateTransferred = DateTime.Parse("Jul 1, 2015"),
                    Comment = "Administratively closed",
                    TransferredByUser = user,
                    TransferredFromUser = user,
                    TransferredFromOffice = office,
                    TransferredToOffice = office,
                    TransferredToUser = user,
                    TransitionType = TransitionType.Closed
                },
            };

            return c;
        }
    }
}
