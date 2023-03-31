namespace Cts.Domain.Entities.Concerns;

public interface IConcernRepository : IRepository<Concern, Guid>
{
    /// <summary>
    /// Returns the <see cref="Concern"/> with the given <paramref name="name"/>.
    /// Returns null if the name does not exist.
    /// </summary>
    /// <param name="name">The Name of the Action Type.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>An Action Type entity.</returns>
    Task<Concern?> FindByNameAsync(string name, CancellationToken token = default);
}
