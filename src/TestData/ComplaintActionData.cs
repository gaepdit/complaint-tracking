using Cts.Domain.ComplaintActions;
using Cts.TestData.Constants;

namespace Cts.TestData;

internal static class ComplaintActionData
{
    private static List<ComplaintAction> ComplaintActionSeedItems => new()
    {
        new ComplaintAction(new Guid("00000000-0000-0000-0000-000000000201"))
        {
            ComplaintId = 1,
            ActionDate = DateTime.Now.AddDays(-3).Date,
            ActionType = ActionTypeData.GetActionTypes.ElementAt(0),
            Investigator = TextData.Word,
            DateEntered = DateTime.Now.AddDays(-3),
            Comments = TextData.Paragraph,
        },
        new ComplaintAction(new Guid("00000000-0000-0000-0000-000000000202"))
        {
            ComplaintId = 1,
            ActionDate = DateTime.Now.AddDays(-2).Date,
            ActionType = ActionTypeData.GetActionTypes.ElementAt(1),
            Investigator = null,
            DateEntered = DateTime.Now.AddDays(-1),
            Comments = null,
        },
        new ComplaintAction(new Guid("00000000-0000-0000-0000-000000000203"))
        {
            ComplaintId = 1,
            ActionDate = DateTime.Now.AddDays(-1).Date,
            ActionType = ActionTypeData.GetActionTypes.ElementAt(5),
            Investigator = TextData.AnotherWord,
            DateEntered = DateTime.Now.AddDays(-1),
            Comments = TextData.MultipleParagraphs,
        },
        new ComplaintAction(new Guid("00000000-0000-0000-0000-000000000204"))
        {
            ComplaintId = 3,
            ActionDate = DateTime.Now.AddDays(-10).Date,
            ActionType = ActionTypeData.GetActionTypes.ElementAt(7),
            Investigator = "Deleted complaint action",
            DateEntered = DateTime.Now.AddDays(-10),
            Comments = TextData.Phrase,
        },
        new ComplaintAction(new Guid("00000000-0000-0000-0000-000000000205"))
        {
            ComplaintId = 4,
            ActionDate = DateTime.Now.AddDays(-1).Date,
            ActionType = ActionTypeData.GetActionTypes.ElementAt(0),
            Investigator = "Complaint action on a deleted complaint",
            DateEntered = DateTime.Now.AddDays(-1),
            Comments = TextData.Phrase,
        },
    };

    private static IEnumerable<ComplaintAction>? _complaintActions;

    public static IEnumerable<ComplaintAction> GetComplaintActions
    {
        get
        {
            if (_complaintActions is not null) return _complaintActions;

            _complaintActions = ComplaintActionSeedItems;
            _complaintActions.ElementAt(3).SetDeleted(null);
            return _complaintActions;
        }
    }

    public static void ClearData() => _complaintActions = null;
}
