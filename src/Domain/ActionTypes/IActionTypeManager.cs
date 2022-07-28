using System.Runtime.Serialization;

namespace Cts.Domain.ActionTypes;

public interface IActionTypeManager
{
    Task<ActionType> CreateAsync(string name);
    Task ChangeNameAsync(ActionType actionType, string name);
}

/// <summary>
/// The exception that is thrown if an action type is added/updated with a name that already exists.
/// </summary>
[Serializable]
public class ActionTypeAlreadyExistsException : Exception
{
    public ActionTypeAlreadyExistsException(string name)
        : base($"An Action Type with that name already exists. Name: {name}") { }

    protected ActionTypeAlreadyExistsException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}
