using Cts.Domain.Entities.ComplaintTransitions;
using Cts.TestData.Constants;
using Cts.TestData.Identity;

namespace Cts.TestData;

internal static class ComplaintTransitionData
{
    private static List<ComplaintTransition> ComplaintTransitionSeedItems => new()
    {
        new ComplaintTransition(new Guid("00000000-0000-0000-0000-000000000301"))
        {
            Complaint = ComplaintData.GetComplaints.ElementAt(0),
            TransitionType = TransitionType.New,
            CommittedDate = DateTimeOffset.Now.AddDays(-10),
            CommittedByUser = UserData.GetUsers.ElementAt(2),
            TransferredToOffice = OfficeData.GetOffices.ElementAt(0),
            Comment = TextData.Phrase,
        },
        new ComplaintTransition(new Guid("00000000-0000-0000-0000-000000000302"))
        {
            Complaint = ComplaintData.GetComplaints.ElementAt(0),
            TransitionType = TransitionType.Assigned,
            CommittedDate = DateTimeOffset.Now.AddDays(-10),
            CommittedByUser = UserData.GetUsers.ElementAt(1),
            TransferredFromOffice = OfficeData.GetOffices.ElementAt(0),
            TransferredToOffice = OfficeData.GetOffices.ElementAt(0),
            TransferredToUser = UserData.GetUsers.ElementAt(2),
        },

        new ComplaintTransition(new Guid("00000000-0000-0000-0000-000000000303"))
        {
            Complaint = ComplaintData.GetComplaints.ElementAt(0),
            TransitionType = TransitionType.Assigned,
            CommittedDate = DateTimeOffset.Now.AddDays(-9),
            CommittedByUser = UserData.GetUsers.ElementAt(2),
            TransferredFromOffice = OfficeData.GetOffices.ElementAt(0),
            TransferredFromUser = UserData.GetUsers.ElementAt(2),
            TransferredToOffice = OfficeData.GetOffices.ElementAt(0),
            TransferredToUser = UserData.GetUsers.ElementAt(1),
        },
        new ComplaintTransition(new Guid("00000000-0000-0000-0000-000000000304"))
        {
            Complaint = ComplaintData.GetComplaints.ElementAt(0),
            TransitionType = TransitionType.Accepted,
            CommittedDate = DateTimeOffset.Now.AddDays(-9),
            CommittedByUser = UserData.GetUsers.ElementAt(1),
        },
        new ComplaintTransition(new Guid("00000000-0000-0000-0000-000000000305"))
        {
            Complaint = ComplaintData.GetComplaints.ElementAt(0),
            TransitionType = TransitionType.SubmittedForReview,
            CommittedDate = DateTimeOffset.Now.AddDays(-7),
            CommittedByUser = UserData.GetUsers.ElementAt(1),
            TransferredFromOffice = OfficeData.GetOffices.ElementAt(0),
            TransferredFromUser = UserData.GetUsers.ElementAt(1),
            TransferredToOffice = OfficeData.GetOffices.ElementAt(0),
            TransferredToUser = UserData.GetUsers.ElementAt(2),
        },
        new ComplaintTransition(new Guid("00000000-0000-0000-0000-000000000306"))
        {
            Complaint = ComplaintData.GetComplaints.ElementAt(0),
            TransitionType = TransitionType.ReturnedByReviewer,
            CommittedDate = DateTimeOffset.Now.AddDays(-6),
            CommittedByUser = UserData.GetUsers.ElementAt(2),
            TransferredFromOffice = OfficeData.GetOffices.ElementAt(0),
            TransferredFromUser = UserData.GetUsers.ElementAt(2),
            TransferredToOffice = OfficeData.GetOffices.ElementAt(0),
            TransferredToUser = UserData.GetUsers.ElementAt(1),
        },
        new ComplaintTransition(new Guid("00000000-0000-0000-0000-000000000307"))
        {
            Complaint = ComplaintData.GetComplaints.ElementAt(0),
            TransitionType = TransitionType.Accepted,
            CommittedDate = DateTimeOffset.Now.AddDays(-5),
            CommittedByUser = UserData.GetUsers.ElementAt(1),
        },
        new ComplaintTransition(new Guid("00000000-0000-0000-0000-000000000308"))
        {
            Complaint = ComplaintData.GetComplaints.ElementAt(0),
            TransitionType = TransitionType.SubmittedForReview,
            CommittedDate = DateTimeOffset.Now.AddDays(-4),
            CommittedByUser = UserData.GetUsers.ElementAt(1),
            TransferredFromOffice = OfficeData.GetOffices.ElementAt(0),
            TransferredFromUser = UserData.GetUsers.ElementAt(1),
            TransferredToOffice = OfficeData.GetOffices.ElementAt(0),
            TransferredToUser = UserData.GetUsers.ElementAt(2),
        },
        new ComplaintTransition(new Guid("00000000-0000-0000-0000-000000000309"))
        {
            Complaint = ComplaintData.GetComplaints.ElementAt(0),
            TransitionType = TransitionType.Closed,
            CommittedDate = DateTimeOffset.Now.AddDays(-3),
            CommittedByUser = UserData.GetUsers.ElementAt(2),
            Comment = TextData.Paragraph,
        },
    };

    private static IEnumerable<ComplaintTransition>? _complaintTransitions;

    public static IEnumerable<ComplaintTransition> GetComplaintTransitions
    {
        get
        {
            if (_complaintTransitions is not null) return _complaintTransitions;
            _complaintTransitions = ComplaintTransitionSeedItems;
            return _complaintTransitions;
        }
    }

    public static void ClearData() => _complaintTransitions = null;
}
