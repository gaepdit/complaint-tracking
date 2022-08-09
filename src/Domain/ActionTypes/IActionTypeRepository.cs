using Cts.Domain.Entities;
using GaEpd.Library.Domain.Repositories;

namespace Cts.Domain.ActionTypes;

public interface IActionTypeRepository : IRepository<ActionType, Guid>
{
    /// <summary>
    /// Returns the <see cref="ActionType"/> with the given <paramref name="name"/>.
    /// Returns null if the name does not exist.
    /// </summary>
    /// <param name="name">The Name of the Action Type.</param>
    /// <param name="cancellationToken"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>An Action Type entity.</returns>
    Task<ActionType?> FindByNameAsync(string name, CancellationToken cancellationToken = default);
}
