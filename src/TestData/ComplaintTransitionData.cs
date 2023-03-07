using Cts.Domain.ComplaintTransitions;
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
            DateTransferred = DateTimeOffset.Now.AddDays(-4),
            TransferredByUser = UserData.GetUsers.ElementAt(2),
            TransferredToOffice = OfficeData.GetOffices.ElementAt(0),
            Comment = TextData.Phrase,
        },
        new ComplaintTransition(new Guid("00000000-0000-0000-0000-000000000302"))
        {
            Complaint = ComplaintData.GetComplaints.ElementAt(0),
            TransitionType = TransitionType.Assigned,
            DateTransferred = DateTimeOffset.Now.AddDays(-4),
            TransferredByUser = UserData.GetUsers.ElementAt(1),
            TransferredFromOffice = OfficeData.GetOffices.ElementAt(0),
            TransferredToOffice = OfficeData.GetOffices.ElementAt(0),
            TransferredToUser = UserData.GetUsers.ElementAt(2),
            DateAccepted = DateTimeOffset.Now.AddDays(-3),
        },
        new ComplaintTransition(new Guid("00000000-0000-0000-0000-000000000303"))
        {
            Complaint = ComplaintData.GetComplaints.ElementAt(0),
            TransitionType = TransitionType.Assigned,
            DateTransferred = DateTimeOffset.Now.AddDays(-3),
            TransferredByUser = UserData.GetUsers.ElementAt(1),
            TransferredFromOffice = OfficeData.GetOffices.ElementAt(0),
            TransferredFromUser = UserData.GetUsers.ElementAt(2),
            TransferredToOffice = OfficeData.GetOffices.ElementAt(0),
            TransferredToUser = UserData.GetUsers.ElementAt(1),
            DateAccepted = DateTimeOffset.Now.AddDays(-3),
        },
        new ComplaintTransition(new Guid("00000000-0000-0000-0000-000000000304"))
        {
            Complaint = ComplaintData.GetComplaints.ElementAt(0),
            TransitionType = TransitionType.SubmittedForReview,
            DateTransferred = DateTimeOffset.Now.AddDays(-2),
            TransferredByUser = UserData.GetUsers.ElementAt(1),
            TransferredFromOffice = OfficeData.GetOffices.ElementAt(0),
            TransferredFromUser = UserData.GetUsers.ElementAt(1),
            TransferredToOffice = OfficeData.GetOffices.ElementAt(0),
            TransferredToUser = UserData.GetUsers.ElementAt(1),
        },
        new ComplaintTransition(new Guid("00000000-0000-0000-0000-000000000305"))
        {
            Complaint = ComplaintData.GetComplaints.ElementAt(0),
            TransitionType = TransitionType.Closed,
            DateTransferred = DateTimeOffset.Now.AddDays(-1),
            TransferredByUser = UserData.GetUsers.ElementAt(1),
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
