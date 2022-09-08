using Cts.Domain.Entities;

namespace Cts.TestData.ActionTypes;

internal static class ActionTypeData
{
    private static readonly List<ActionType> ActionTypeSeedItems = new()
    {
        new ActionType(new Guid("00000000-0000-0000-0000-000000000008"), "Initial investigation"),
        new ActionType(new Guid("00000000-0000-0000-0000-000000000009"), "Follow-up investigation"),
        new ActionType(new Guid("00000000-0000-0000-0000-000000000010"), "Referred to"),
        new ActionType(new Guid("00000000-0000-0000-0000-000000000011"), "Notice of violation"),
        new ActionType(new Guid("00000000-0000-0000-0000-000000000012"), "Initial investigation report"),
        new ActionType(new Guid("00000000-0000-0000-0000-000000000013"), "Follow-up investigation report"),
        new ActionType(new Guid("00000000-0000-0000-0000-000000000014"), "Status letter"),
        new ActionType(new Guid("00000000-0000-0000-0000-000000000015"), "Other"),
        new ActionType(new Guid("00000000-0000-0000-0000-000000000016"), "Consent/administrative order"),
    };

    private static ICollection<ActionType>? _actionTypes;

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
