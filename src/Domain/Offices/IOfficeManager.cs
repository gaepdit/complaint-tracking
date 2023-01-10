using Cts.Domain.Exceptions;
using Cts.Domain.Identity;

namespace Cts.Domain.Offices;

/// <summary>
/// A manager for managing Offices.
/// </summary>
public interface IOfficeManager
{
    /// <summary>
    /// Creates a new <see cref="Office"/>.
    /// </summary>
    /// <param name="name">The name of the Office to create.</param>
    /// <param name="user">The <see cref="Office.Assignor"/> for the Office.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <exception cref="NameAlreadyExistsException">Thrown if an Office already exists with the given name.</exception>
    /// <returns>The Office that was created.</returns>
    Task<Office> CreateAsync(string name, ApplicationUser? user = null, CancellationToken token = default);

    /// <summary>
    /// Changes the name of an <see cref="Office"/>.
    /// </summary>
    /// <param name="office">The Office to modify.</param>
    /// <param name="name">The new name for the Office.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <exception cref="NameAlreadyExistsException">Thrown if an Office already exists with the given name.</exception>
    Task ChangeNameAsync(Office office, string name, CancellationToken token = default);
}
