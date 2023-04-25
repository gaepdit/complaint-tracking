using Cts.Domain.Entities.Concerns;
using Cts.TestData;

namespace Cts.LocalRepository.Repositories;

/// <inheritdoc cref="IConcernRepository" />
public sealed class LocalConcernRepository : BaseRepository<Concern, Guid>, IConcernRepository
{
    public LocalConcernRepository() : base(ConcernData.GetConcerns) { }

    public Task<Concern?> FindByNameAsync(string name, CancellationToken token = default) =>
        Task.FromResult(Items.SingleOrDefault(e => string.Equals(e.Name.ToUpper(), name.ToUpper())));
}
