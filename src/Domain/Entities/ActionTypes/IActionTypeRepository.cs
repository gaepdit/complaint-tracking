namespace Cts.Domain.Entities.ActionTypes;

public interface IActionTypeRepository : IRepository<ActionType, Guid>
{
    /// <summary>
    /// Returns the <see cref="ActionType"/> with the given <paramref name="name"/>.
    /// Returns null if the name does not exist.
    /// </summary>
    /// <param name="name">The Name of the Action Type.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>An Action Type entity.</returns>
    Task<ActionType?> FindByNameAsync(string name, CancellationToken token = default);
}
