using System.Runtime.Serialization;

namespace MyAppRoot.Domain.Offices;

/// <summary>
/// A manager for managing Offices.
/// </summary>
public interface IOfficeManager
{
    /// <summary>
    /// Creates a new <see cref="Office"/>.
    /// </summary>
    /// <param name="name">The name of the Office to create.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <exception cref="OfficeNameAlreadyExistsException">Thrown if an Office already exists with the given name.</exception>
    /// <returns>The Office that was created.</returns>
    Task<Office> CreateAsync(string name, CancellationToken token = default);

    /// <summary>
    /// Changes the name of an <see cref="Office"/>.
    /// </summary>
    /// <param name="office">The Office to modify.</param>
    /// <param name="name">The new name for the Office.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <exception cref="OfficeNameAlreadyExistsException">Thrown if an Office already exists with the given name.</exception>
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
