using Cts.Domain.Entities.ComplaintTransitions;
using Cts.TestData.Constants;
using Cts.TestData.Identity;

namespace Cts.TestData;

internal static class ComplaintTransitionData
{
    private static IEnumerable<ComplaintTransition> ComplaintTransitionSeedItems => new List<ComplaintTransition>
    {
        new(new Guid("40000000-0000-0000-0000-000000000001"),
            ComplaintData.GetComplaints.ElementAt(0),
            TransitionType.New,
            UserData.GetUsers.ElementAt(2))
        {
            CommittedDate = DateTimeOffset.Now.AddDays(-10),
            TransferredToOffice = OfficeData.GetOffices.ElementAt(0),
            Comment = TextData.Phrase,
        },
        new(new Guid("40000000-0000-0000-0000-000000000002"),
            ComplaintData.GetComplaints.ElementAt(0),
            TransitionType.Assigned,
            UserData.GetUsers.ElementAt(1))
        {
            CommittedDate = DateTimeOffset.Now.AddDays(-10),
            TransferredToOffice = OfficeData.GetOffices.ElementAt(0),
            TransferredToUser = UserData.GetUsers.ElementAt(2),
        },
        new(new Guid("40000000-0000-0000-0000-000000000003"),
            ComplaintData.GetComplaints.ElementAt(0),
            TransitionType.Assigned,
            UserData.GetUsers.ElementAt(2))
        {
            CommittedDate = DateTimeOffset.Now.AddDays(-9),
            TransferredToOffice = OfficeData.GetOffices.ElementAt(0),
            TransferredToUser = UserData.GetUsers.ElementAt(1),
        },
        new(new Guid("40000000-0000-0000-0000-000000000004"),
            ComplaintData.GetComplaints.ElementAt(0),
            TransitionType.Accepted,
            UserData.GetUsers.ElementAt(1))
        {
            CommittedDate = DateTimeOffset.Now.AddDays(-9),
        },
        new(new Guid("40000000-0000-0000-0000-000000000005"),
            ComplaintData.GetComplaints.ElementAt(0),
            TransitionType.SubmittedForReview,
            UserData.GetUsers.ElementAt(1))
        {
            CommittedDate = DateTimeOffset.Now.AddDays(-7),
            TransferredToOffice = OfficeData.GetOffices.ElementAt(0),
            TransferredToUser = UserData.GetUsers.ElementAt(2),
        },
        new(new Guid("40000000-0000-0000-0000-000000000006"),
            ComplaintData.GetComplaints.ElementAt(0),
            TransitionType.ReturnedByReviewer,
            UserData.GetUsers.ElementAt(2))
        {
            CommittedDate = DateTimeOffset.Now.AddDays(-6),
            TransferredToOffice = OfficeData.GetOffices.ElementAt(0),
            TransferredToUser = UserData.GetUsers.ElementAt(1),
        },
        new(new Guid("40000000-0000-0000-0000-000000000007"),
            ComplaintData.GetComplaints.ElementAt(0),
            TransitionType.Accepted,
            UserData.GetUsers.ElementAt(1))
        {
            CommittedDate = DateTimeOffset.Now.AddDays(-5),
        },
        new(new Guid("40000000-0000-0000-0000-000000000008"),
            ComplaintData.GetComplaints.ElementAt(0),
            TransitionType.SubmittedForReview,
            UserData.GetUsers.ElementAt(1))
        {
            CommittedDate = DateTimeOffset.Now.AddDays(-4),
            TransferredToOffice = OfficeData.GetOffices.ElementAt(0),
            TransferredToUser = UserData.GetUsers.ElementAt(2),
        },
        new(new Guid("40000000-0000-0000-0000-000000000009"),
            ComplaintData.GetComplaints.ElementAt(0),
            TransitionType.Closed,
            UserData.GetUsers.ElementAt(2))
        {
            CommittedDate = DateTimeOffset.Now.AddDays(-3),
            Comment = TextData.Paragraph,
        },
        new(new Guid("40000000-0000-0000-0000-000000000010"),
            ComplaintData.GetComplaints.ElementAt(7),
            TransitionType.New,
            UserData.GetUsers.ElementAt(0))
        {
            CommittedDate = DateTimeOffset.Now.AddDays(-1),
        },
        new(new Guid("40000000-0000-0000-0000-000000000011"),
            ComplaintData.GetComplaints.ElementAt(7),
            TransitionType.Assigned,
            UserData.GetUsers.ElementAt(0))
        {
            CommittedDate = DateTimeOffset.Now.AddDays(-1),
            TransferredToOffice = OfficeData.GetOffices.ElementAt(1),
            TransferredToUser = UserData.GetUsers.ElementAt(1),
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
