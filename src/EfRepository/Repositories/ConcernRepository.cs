using Cts.Domain.Entities.Concerns;

namespace Cts.EfRepository.Repositories;

public sealed class ConcernRepository : NamedEntityRepository<Concern>, IConcernRepository
{
    public ConcernRepository(DbContext dbContext) : base(dbContext) { }
}
