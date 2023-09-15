using Cts.Domain.Exceptions;

namespace Cts.Domain.Entities.ActionTypes;

/// <summary>
/// A manager for managing Action Types.
/// </summary>
public interface IActionTypeManager
{
    /// <summary>
    /// Creates a new <see cref="ActionType"/>.
    /// Throws <see cref="NameAlreadyExistsException"/> if an Action Type already exists with the given name.
    /// </summary>
    /// <param name="name">The name of the Action Type to create.</param>
    /// <param name="createdById">The ID of the user creating the entity.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>The Action Type that was created.</returns>
    Task<ActionType> CreateAsync(string name, string? createdById, CancellationToken token = default);

    /// <summary>
    /// Changes the name of an <see cref="ActionType"/>.
    /// Throws <see cref="NameAlreadyExistsException"/> if another Action Type already exists with the
    /// given name.
    /// </summary>
    /// <param name="actionType">The Action Type to modify.</param>
    /// <param name="name">The new name for the Action Type.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    Task ChangeNameAsync(ActionType actionType, string name, CancellationToken token = default);
}
