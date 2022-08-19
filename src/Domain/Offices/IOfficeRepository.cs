using Cts.Domain.Entities;
using GaEpd.Library.Domain.Repositories;

namespace Cts.Domain.Offices;

public interface IOfficeRepository : IRepository<Office, Guid>
{
    /// <summary>
    /// Returns the <see cref="Office"/> with the given <paramref name="name"/>.
    /// Returns null if the name does not exist.
    /// </summary>
    /// <param name="name">The Name of the Office.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>An Office entity.</returns>
    Task<Office?> FindByNameAsync(string name, CancellationToken token = default);

    /// <summary>
    /// Returns a list of all active <see cref="ApplicationUser"/> located in the <see cref="Office"/> with the
    /// given <paramref name="id"/>.
    /// Throws <see cref="EntityNotFoundException"/> if no entity exists with the given Id.
    /// </summary>
    /// <param name="id">The ID of the Office.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>A list of Users.</returns>
    Task<List<ApplicationUser>> GetActiveUsersListAsync(Guid id, CancellationToken token = default);
}
