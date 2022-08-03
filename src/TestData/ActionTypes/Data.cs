﻿using Cts.Domain.ActionTypes;

namespace Cts.TestData.ActionTypes;

internal static class Data
{
    private static readonly string[] ActionTypeItems =
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

    public static IEnumerable<ActionType> ActionTypes
    {
        get
        {
            if (_actionTypes is not null) return _actionTypes;
            _actionTypes = ActionTypeItems.Select(i => new ActionType(Guid.NewGuid(), i)).ToList();
            return _actionTypes;
        }
    }
}