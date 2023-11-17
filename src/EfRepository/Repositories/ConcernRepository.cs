using Cts.Domain.Entities.Concerns;

namespace Cts.EfRepository.Repositories;

public sealed class ConcernRepository(AppDbContext dbContext) :
    NamedEntityRepository<Concern, AppDbContext>(dbContext), IConcernRepository;
