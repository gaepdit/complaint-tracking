using Cts.Domain.ActionTypes;

namespace Cts.TestData;

internal static class ActionTypeData
{
    private static readonly List<ActionType> ActionTypeSeedItems = new()
    {
        new ActionType(new Guid("00000000-0000-0000-0000-000000000020"), "Initial investigation"),
        new ActionType(new Guid("00000000-0000-0000-0000-000000000021"), "Follow-up investigation"),
        new ActionType(new Guid("00000000-0000-0000-0000-000000000022"), "Referred to"),
        new ActionType(new Guid("00000000-0000-0000-0000-000000000023"), "Notice of violation"),
        new ActionType(new Guid("00000000-0000-0000-0000-000000000024"), "Initial investigation report"),
        new ActionType(new Guid("00000000-0000-0000-0000-000000000025"), "Follow-up investigation report"),
        new ActionType(new Guid("00000000-0000-0000-0000-000000000026"), "Status letter"),
        new ActionType(new Guid("00000000-0000-0000-0000-000000000027"), "Other"),
        new ActionType(new Guid("00000000-0000-0000-0000-000000000028"), "Consent/administrative order"),
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
}
