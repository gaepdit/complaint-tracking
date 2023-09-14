using Cts.Domain.Exceptions;

namespace Cts.Domain.Entities.Concerns;

/// <summary>
/// A manager for managing Concerns.
/// </summary>
public interface IConcernManager
{
    /// <summary>
    /// Creates a new <see cref="Concern"/>.
    /// Throws <see cref="NameAlreadyExistsException"/> if a Concern already exists with the given name.
    /// </summary>
    /// <param name="name">The name of the Concern to create.</param>
    /// <param name="createdById">The ID of the user creating the entity.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>The Concern that was created.</returns>
    Task<Concern> CreateAsync(string name, string? createdById, CancellationToken token = default);

    /// <summary>
    /// Changes the name of an <see cref="Concern"/>.
    /// Throws <see cref="NameAlreadyExistsException"/> if another Concern already exists with the
    /// given name.
    /// </summary>
    /// <param name="concern">The Concern to modify.</param>
    /// <param name="name">The new name for the Concern.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    Task ChangeNameAsync(Concern concern, string name, CancellationToken token = default);
}
