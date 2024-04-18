using Cts.Domain.Entities.ComplaintActions;
using Cts.TestData.Constants;
using Cts.TestData.Identity;

namespace Cts.TestData;

internal static class ComplaintActionData
{
    private static IEnumerable<ComplaintAction> ComplaintActionSeedItems => new List<ComplaintAction>
    {
        new(new Guid("30000000-0000-0000-0000-000000000001"), // 0
            ComplaintData.GetComplaints.ElementAt(0),
            ActionTypeData.GetActionTypes.ElementAt(0))
        {
            ActionDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddDays(-3).Date),
            Investigator = TextData.Word,
            EnteredDate = DateTimeOffset.Now.AddDays(-3),
            EnteredBy = UserData.GetUsers.ElementAt(1),
            Comments = $"Email: {TextData.ValidEmail} & Phone: {TextData.ValidPhoneNumber}",
        },
        new(new Guid("30000000-0000-0000-0000-000000000002"), // 1
            ComplaintData.GetComplaints.ElementAt(0),
            ActionTypeData.GetActionTypes.ElementAt(1))
        {
            ActionDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddDays(-2).Date),
            Investigator = TextData.EmojiWord,
            EnteredDate = DateTimeOffset.Now.AddDays(-1),
            EnteredBy = UserData.GetUsers.ElementAt(0),
            Comments = TextData.EmojiWord,
        },
        new(new Guid("30000000-0000-0000-0000-000000000003"), // 2
            ComplaintData.GetComplaints.ElementAt(0),
            ActionTypeData.GetActionTypes.ElementAt(5))
        {
            ActionDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddDays(-1).Date),
            Investigator = TextData.AnotherWord,
            EnteredDate = DateTimeOffset.Now.AddDays(-1),
            EnteredBy = UserData.GetUsers.ElementAt(1),
            Comments = TextData.MultipleParagraphs,
        },
        new(new Guid("30000000-0000-0000-0000-000000000004"), // 3
            ComplaintData.GetComplaints.ElementAt(0),
            ActionTypeData.GetActionTypes.ElementAt(7))
        {
            ActionDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddDays(-2).Date),
            Investigator = "Deleted complaint action on closed complaint",
            EnteredDate = DateTimeOffset.Now.AddDays(-2),
            EnteredBy = UserData.GetUsers.ElementAt(0),
            Comments = TextData.Phrase,
        },
        new(new Guid("30000000-0000-0000-0000-000000000005"), // 4
            ComplaintData.GetComplaints.ElementAt(3),
            ActionTypeData.GetActionTypes.ElementAt(0))
        {
            ActionDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddDays(-1).Date),
            Investigator = "Complaint action on a deleted complaint",
            EnteredDate = DateTimeOffset.Now.AddDays(-1),
            EnteredBy = UserData.GetUsers.ElementAt(1),
            Comments = TextData.Phrase,
        },
        new(new Guid("30000000-0000-0000-0000-000000000006"), // 5
            ComplaintData.GetComplaints.ElementAt(5),
            ActionTypeData.GetActionTypes.ElementAt(0))
        {
            ActionDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddDays(-2).Date),
            Investigator = "Action on current complaint",
            EnteredDate = DateTimeOffset.Now.AddDays(-2),
            EnteredBy = UserData.GetUsers.ElementAt(1),
            Comments = TextData.Paragraph,
        },
        new(new Guid("30000000-0000-0000-0000-000000000007"), // 6
            ComplaintData.GetComplaints.ElementAt(5),
            ActionTypeData.GetActionTypes.ElementAt(7))
        {
            ActionDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddDays(-3).Date),
            Investigator = "Deleted complaint action on current complaint",
            EnteredDate = DateTimeOffset.Now.AddDays(-2),
            EnteredBy = UserData.GetUsers.ElementAt(0),
            Comments = TextData.Phrase,
        },
        new(new Guid("30000000-0000-0000-0000-000000000008"), // 7
            ComplaintData.GetComplaints.ElementAt(3),
            ActionTypeData.GetActionTypes.ElementAt(7))
        {
            ActionDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddDays(-3).Date),
            Investigator = "Deleted action on deleted complaint",
            EnteredDate = DateTimeOffset.Now.AddDays(-2),
            EnteredBy = UserData.GetUsers.ElementAt(0),
            Comments = TextData.Phrase,
        },
        new(new Guid("30000000-0000-0000-0000-000000000009"), // 8
            ComplaintData.GetComplaints.ElementAt(0),
            ActionTypeData.GetActionTypes.ElementAt(5))
        {
            ActionDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddDays(-1).Date),
            Investigator = "Complaint Action with null entered date",
            EnteredDate = null,
            EnteredBy = UserData.GetUsers.ElementAt(1),
            Comments = TextData.MultipleParagraphs,
        },
    };

    private static List<ComplaintAction>? _complaintActions;

    public static IEnumerable<ComplaintAction> GetComplaintActions
    {
        get
        {
            if (_complaintActions is not null) return _complaintActions;

            _complaintActions = ComplaintActionSeedItems.ToList();
            _complaintActions[3].SetDeleted("00000000-0000-0000-0000-000000000001");
            _complaintActions[6].SetDeleted("00000000-0000-0000-0000-000000000001");
            _complaintActions[7].SetDeleted("00000000-0000-0000-0000-000000000001");
            return _complaintActions;
        }
    }

    public static void ClearData() => _complaintActions = null;
}
