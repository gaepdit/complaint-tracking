using Cts.Domain.Entities.Concerns;

namespace Cts.EfRepository.Repositories;

public sealed class ConcernRepository : NamedEntityRepository<Concern, AppDbContext>, IConcernRepository
{
    public ConcernRepository(AppDbContext dbContext) : base(dbContext) { }
}
