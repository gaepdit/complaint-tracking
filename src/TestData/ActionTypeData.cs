using Cts.Domain.Entities.ActionTypes;

namespace Cts.TestData;

internal static class ActionTypeData
{
    private static IEnumerable<ActionType> ActionTypeSeedItems => new List<ActionType>
    {
        new(new Guid("10000000-0000-0000-0000-000000000001"), "Initial investigation"),
        new(new Guid("10000000-0000-0000-0000-000000000002"), "Follow-up investigation"),
        new(new Guid("10000000-0000-0000-0000-000000000003"), "Referred to"),
        new(new Guid("10000000-0000-0000-0000-000000000004"), "Notice of violation"),
        new(new Guid("10000000-0000-0000-0000-000000000005"), "Initial investigation report"),
        new(new Guid("10000000-0000-0000-0000-000000000006"), "Follow-up investigation report"),
        new(new Guid("10000000-0000-0000-0000-000000000007"), "Status letter"),
        new(new Guid("10000000-0000-0000-0000-000000000008"), "Other"),
        new(new Guid("10000000-0000-0000-0000-000000000009"), "Consent/administrative order"),
    };

    private static IEnumerable<ActionType>? _actionTypes;

    public static IEnumerable<ActionType> GetActionTypes
    {
        get
        {
            if (_actionTypes is not null) return _actionTypes;
            _actionTypes = ActionTypeSeedItems;
            return _actionTypes;
        }
    }

    public static void ClearData() => _actionTypes = null;
}
