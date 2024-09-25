using Cts.Domain.Identity;

namespace Cts.Domain.Entities.Offices;

public interface IOfficeRepository : INamedEntityRepository<Office>
{
    /// <summary>
    /// Returns a list of all active <see cref="ApplicationUser"/> located in the <see cref="Office"/> with the
    /// given <paramref name="id"/>.
    /// </summary>
    /// <param name="id">The ID of the Office.</param>
    /// <param name="includeInactive">A flag indicating whether to include inactive Staff Members.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <exception cref="EntityNotFoundException{Office}">Thrown if no entity exists with the given ID.</exception>
    /// <returns>A list of Users.</returns>
    Task<List<ApplicationUser>> GetStaffMembersListAsync(Guid id, bool includeInactive,
        CancellationToken token = default);

    /// <summary>
    /// Returns the <see cref="Office"/> with the given <paramref name="id"/>
    /// and includes the assignor property.
    /// Returns null if no entity exists with the given ID.
    /// </summary>
    /// <param name="id">The ID of the entity.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>An Office.</returns>
    Task<Office?> FindIncludeAssignorAsync(Guid id, CancellationToken token = default);

    /// <summary>
    /// Returns a read-only collection of all <see cref="Office"/> values>
    /// and includes the assignor property.
    /// Returns an empty collection if none exist.
    /// </summary>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>A read-only collection of Offices.</returns>
    Task<IReadOnlyCollection<Office>> GetListIncludeAssignorAsync(CancellationToken token = default);
}
