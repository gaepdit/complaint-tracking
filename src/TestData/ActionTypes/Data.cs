using Cts.Domain.Entities;

namespace Cts.TestData.ActionTypes;

internal static class Data
{
    private static readonly string[] ActionTypeSeedItems =
    {
        "Initial investigation",
        "Follow-up investigation",
        "Referred to",
        "Notice of violation",
        "Initial investigation report",
        "Follow-up investigation report",
        "Status letter",
        "Other",
        "Consent/administrative order",
    };

    private static ICollection<ActionType>? _actionTypes;

    public static IEnumerable<ActionType> GetActionTypes
    {
        get
        {
            if (_actionTypes is not null) return _actionTypes;
            _actionTypes = ActionTypeSeedItems.Select(i => new ActionType(Guid.NewGuid(), i)).ToList();
            return _actionTypes;
        }
    }
}
