using Cts.Domain.Entities;
using Cts.Domain.Users;
using System.Runtime.Serialization;

namespace Cts.Domain.Offices;

/// <summary>
/// A manager for managing Offices.
/// </summary>
public interface IOfficeManager
{
    /// <summary>
    /// Creates a new <see cref="Office"/>.
    /// Throws <see cref="OfficeNameAlreadyExistsException"/> if an Office already exists with the given name.
    /// </summary>
    /// <param name="name">The name of the Office to create.</param>
    /// <param name="user">The <see cref="Office.MasterUser"/> for the Office.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>The Office that was created.</returns>
    Task<Office> CreateAsync(string name, ApplicationUser? user = null, CancellationToken token = default);
    
    /// <summary>
    /// Changes the name of an <see cref="Office"/>.
    /// Throws <see cref="OfficeNameAlreadyExistsException"/> if another Office already exists with the
    /// given name.
    /// </summary>
    /// <param name="office">The Office to modify.</param>
    /// <param name="name">The new name for the Office.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    Task ChangeNameAsync(Office office, string name, CancellationToken token = default);
}

/// <summary>
/// The exception that is thrown if an <see cref="Office"/> is added/updated with a name that already exists.
/// </summary>
[Serializable]
public class OfficeNameAlreadyExistsException : Exception
{
    public OfficeNameAlreadyExistsException(string name)
        : base($"An Office with that name already exists. Name: {name}") { }

    protected OfficeNameAlreadyExistsException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}
