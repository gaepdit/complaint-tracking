using Cts.Domain.Entities.ComplaintTransitions;
using Cts.TestData.Constants;
using Cts.TestData.Identity;

namespace Cts.TestData;

internal static class ComplaintTransitionData
{
    private static IEnumerable<ComplaintTransition> ComplaintTransitionSeedItems => new List<ComplaintTransition>()
    {
        new(new Guid("00000000-0000-0000-0000-000000000301"),
            ComplaintData.GetComplaints.ElementAt(0),
            TransitionType.New,
            UserData.GetUsers.ElementAt(2))
        {
            CommittedDate = DateTimeOffset.Now.AddDays(-10),
            TransferredToOffice = OfficeData.GetOffices.ElementAt(0),
            Comment = TextData.Phrase,
        },
        new(new Guid("00000000-0000-0000-0000-000000000302"),
            ComplaintData.GetComplaints.ElementAt(0),
            TransitionType.Assigned,
            UserData.GetUsers.ElementAt(1))
        {
            CommittedDate = DateTimeOffset.Now.AddDays(-10),
            TransferredFromOffice = OfficeData.GetOffices.ElementAt(0),
            TransferredToOffice = OfficeData.GetOffices.ElementAt(0),
            TransferredToUser = UserData.GetUsers.ElementAt(2),
        },

        new(new Guid("00000000-0000-0000-0000-000000000303"),
            ComplaintData.GetComplaints.ElementAt(0),
            TransitionType.Assigned,
            UserData.GetUsers.ElementAt(2))
        {
            CommittedDate = DateTimeOffset.Now.AddDays(-9),
            TransferredFromOffice = OfficeData.GetOffices.ElementAt(0),
            TransferredFromUser = UserData.GetUsers.ElementAt(2),
            TransferredToOffice = OfficeData.GetOffices.ElementAt(0),
            TransferredToUser = UserData.GetUsers.ElementAt(1),
        },
        new(new Guid("00000000-0000-0000-0000-000000000304"),
            ComplaintData.GetComplaints.ElementAt(0),
            TransitionType.Accepted,
            UserData.GetUsers.ElementAt(1))
        {
            CommittedDate = DateTimeOffset.Now.AddDays(-9),
        },
        new(new Guid("00000000-0000-0000-0000-000000000305"),
            ComplaintData.GetComplaints.ElementAt(0),
            TransitionType.SubmittedForReview,
            UserData.GetUsers.ElementAt(1))
        {
            CommittedDate = DateTimeOffset.Now.AddDays(-7),
            TransferredFromOffice = OfficeData.GetOffices.ElementAt(0),
            TransferredFromUser = UserData.GetUsers.ElementAt(1),
            TransferredToOffice = OfficeData.GetOffices.ElementAt(0),
            TransferredToUser = UserData.GetUsers.ElementAt(2),
        },
        new(new Guid("00000000-0000-0000-0000-000000000306"),
            ComplaintData.GetComplaints.ElementAt(0),
            TransitionType.ReturnedByReviewer,
            UserData.GetUsers.ElementAt(2))
        {
            CommittedDate = DateTimeOffset.Now.AddDays(-6),
            TransferredFromOffice = OfficeData.GetOffices.ElementAt(0),
            TransferredFromUser = UserData.GetUsers.ElementAt(2),
            TransferredToOffice = OfficeData.GetOffices.ElementAt(0),
            TransferredToUser = UserData.GetUsers.ElementAt(1),
        },
        new(new Guid("00000000-0000-0000-0000-000000000307"),
            ComplaintData.GetComplaints.ElementAt(0),
            TransitionType.Accepted,
            UserData.GetUsers.ElementAt(1))
        {
            CommittedDate = DateTimeOffset.Now.AddDays(-5),
        },
        new(new Guid("00000000-0000-0000-0000-000000000308"),
            ComplaintData.GetComplaints.ElementAt(0),
            TransitionType.SubmittedForReview,
            UserData.GetUsers.ElementAt(1))
        {
            CommittedDate = DateTimeOffset.Now.AddDays(-4),
            TransferredFromOffice = OfficeData.GetOffices.ElementAt(0),
            TransferredFromUser = UserData.GetUsers.ElementAt(1),
            TransferredToOffice = OfficeData.GetOffices.ElementAt(0),
            TransferredToUser = UserData.GetUsers.ElementAt(2),
        },
        new(new Guid("00000000-0000-0000-0000-000000000309"),
            ComplaintData.GetComplaints.ElementAt(0),
            TransitionType.Closed,
            UserData.GetUsers.ElementAt(2))
        {
            CommittedDate = DateTimeOffset.Now.AddDays(-3),
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
