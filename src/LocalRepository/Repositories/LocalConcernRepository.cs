using Cts.Domain.Entities.Concerns;
using Cts.TestData;

namespace Cts.LocalRepository.Repositories;

public sealed class LocalConcernRepository()
    : NamedEntityRepository<Concern>(ConcernData.GetConcerns), IConcernRepository;
