using Cts.Domain.Entities.ComplaintActions;
using Cts.TestData.Constants;
using Cts.TestData.Identity;

namespace Cts.TestData;

internal static class ComplaintActionData
{
    private static IEnumerable<ComplaintAction> ComplaintActionSeedItems => new List<ComplaintAction>
    {
        new(new Guid("30000000-0000-0000-0000-000000000001"),
            ComplaintData.GetComplaints.ElementAt(0))
        {
            ActionDate = DateTimeOffset.Now.AddDays(-3).Date,
            ActionType = ActionTypeData.GetActionTypes.ElementAt(0),
            Investigator = TextData.Word,
            EnteredDate = DateTimeOffset.Now.AddDays(-3),
            EnteredBy = UserData.GetUsers.ElementAt(1),
            Comments = TextData.Paragraph,
        },
        new(new Guid("30000000-0000-0000-0000-000000000002"),
            ComplaintData.GetComplaints.ElementAt(0))
        {
            ActionDate = DateTimeOffset.Now.AddDays(-2).Date,
            ActionType = ActionTypeData.GetActionTypes.ElementAt(1),
            Investigator = null,
            EnteredDate = DateTimeOffset.Now.AddDays(-1),
            EnteredBy = UserData.GetUsers.ElementAt(0),
            Comments = null,
        },
        new(new Guid("30000000-0000-0000-0000-000000000003"),
            ComplaintData.GetComplaints.ElementAt(0))
        {
            ActionDate = DateTimeOffset.Now.AddDays(-1).Date,
            ActionType = ActionTypeData.GetActionTypes.ElementAt(5),
            Investigator = TextData.AnotherWord,
            EnteredDate = DateTimeOffset.Now.AddDays(-1),
            EnteredBy = UserData.GetUsers.ElementAt(1),
            Comments = TextData.MultipleParagraphs,
        },
        new(new Guid("30000000-0000-0000-0000-000000000004"),
            ComplaintData.GetComplaints.ElementAt(2))
        {
            ActionDate = DateTimeOffset.Now.AddDays(-10).Date,
            ActionType = ActionTypeData.GetActionTypes.ElementAt(7),
            Investigator = "Deleted complaint action",
            EnteredDate = DateTimeOffset.Now.AddDays(-10),
            EnteredBy = UserData.GetUsers.ElementAt(0),
            Comments = TextData.Phrase,
        },
        new(new Guid("30000000-0000-0000-0000-000000000005"),
            ComplaintData.GetComplaints.ElementAt(3))
        {
            ActionDate = DateTimeOffset.Now.AddDays(-1).Date,
            ActionType = ActionTypeData.GetActionTypes.ElementAt(0),
            Investigator = "Complaint action on a deleted complaint",
            EnteredDate = DateTimeOffset.Now.AddDays(-1),
            EnteredBy = UserData.GetUsers.ElementAt(1),
            Comments = TextData.Phrase,
        },
    };

    private static List<ComplaintAction>? _complaintActions;

    public static IEnumerable<ComplaintAction> GetComplaintActions
    {
        get
        {
            if (_complaintActions is not null) return _complaintActions;

            _complaintActions = ComplaintActionSeedItems.ToList();
            _complaintActions.ElementAt(3).SetDeleted("00000000-0000-0000-0000-000000000001");
            return _complaintActions;
        }
    }

    public static void ClearData() => _complaintActions = null;
}
