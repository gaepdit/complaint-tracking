using System.Runtime.Serialization;

namespace Cts.Domain.ActionTypes;

/// <summary>
/// A manager for managing Action Types.
/// </summary>
public interface IActionTypeManager
{
    /// <summary>
    /// Creates a new <see cref="ActionType"/>.
    /// Throws <see cref="ActionTypeNameAlreadyExistsException"/> if an Action Type already exists with the given name.
    /// </summary>
    /// <param name="name">The name of the Action Type to create.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>The Action Type that was created.</returns>
    Task<ActionType> CreateAsync(string name, CancellationToken token = default);

    /// <summary>
    /// Changes the name of an <see cref="ActionType"/>.
    /// Throws <see cref="ActionTypeNameAlreadyExistsException"/> if another Action Type already exists with the
    /// given name.
    /// </summary>
    /// <param name="actionType">The Action Type to modify.</param>
    /// <param name="name">The new name for the Action Type.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    Task ChangeNameAsync(ActionType actionType, string name, CancellationToken token = default);
}

/// <summary>
/// The exception that is thrown if an <see cref="ActionType"/> is added/updated with a name that already exists.
/// </summary>
[Serializable]
public class ActionTypeNameAlreadyExistsException : Exception
{
    public ActionTypeNameAlreadyExistsException(string name)
        : base($"An Action Type with that name already exists. Name: {name}") { }

    protected ActionTypeNameAlreadyExistsException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}
