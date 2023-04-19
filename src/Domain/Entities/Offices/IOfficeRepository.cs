﻿using Cts.Domain.Identity;

namespace Cts.Domain.Entities.Offices;

/// <inheritdoc />
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
    /// Returns a list of all <see cref="ApplicationUser"/> located in the <see cref="Office"/> with the
    /// given <paramref name="id"/>.
    /// </summary>
    /// <param name="id">The ID of the Office.</param>
    /// <param name="activeOnly">If true, returns only active staff members.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <exception cref="EntityNotFoundException">Thrown if no entity exists with the given Id.</exception>
    /// <returns>A list of Users.</returns>
    Task<List<ApplicationUser>> GetStaffMembersListAsync(
        Guid id, bool activeOnly, CancellationToken token = default);
    
    /// <summary>
    /// Returns the <see cref="Office"/> with the given <paramref name="id"/>
    /// and includes the assignor property.
    /// Returns null if no entity exists with the given Id.
    /// </summary>
    /// <param name="id">The Id of the entity.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>An entity.</returns>
     Task<Office?> FindIncludeAssignorAsync(Guid id, CancellationToken token = default) ;

    /// <summary>
    /// Returns a read-only collection of all <see cref="Office"/> values>
    /// and includes the assignor property.
    /// Returns an empty collection if none exist.
    /// </summary>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>A read-only collection of entities.</returns>
     Task<IReadOnlyCollection<Office>> GetListIncludeAssignorAsync(CancellationToken token = default);
}
