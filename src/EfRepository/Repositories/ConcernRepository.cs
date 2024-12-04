using Cts.Domain.Entities.Concerns;

namespace Cts.EfRepository.Repositories;

public sealed class ConcernRepository(AppDbContext context) :
    NamedEntityRepository<Concern, AppDbContext>(context), IConcernRepository;
