using System.Runtime.Serialization;

namespace Cts.Domain.Concerns;

/// <summary>
/// A manager for managing Concerns.
/// </summary>
public interface IConcernManager
{
    /// <summary>
    /// Creates a new <see cref="Concern"/>.
    /// Throws <see cref="ConcernNameAlreadyExistsException"/> if a Concern already exists with the given name.
    /// </summary>
    /// <param name="name">The name of the Concern to create.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>The Concern that was created.</returns>
    Task<Concern> CreateAsync(string name, CancellationToken token = default);

    /// <summary>
    /// Changes the name of an <see cref="Concern"/>.
    /// Throws <see cref="ConcernNameAlreadyExistsException"/> if another Concern already exists with the
    /// given name.
    /// </summary>
    /// <param name="concern">The Concern to modify.</param>
    /// <param name="name">The new name for the Concern.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    Task ChangeNameAsync(Concern concern, string name, CancellationToken token = default);
}

/// <summary>
/// The exception that is thrown if an <see cref="Concern"/> is added/updated with a name that already exists.
/// </summary>
[Serializable]
public class ConcernNameAlreadyExistsException : Exception
{
    public ConcernNameAlreadyExistsException(string name)
        : base($"An Concern with that name already exists. Name: {name}") { }

    protected ConcernNameAlreadyExistsException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}
