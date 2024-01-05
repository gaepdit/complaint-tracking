namespace Cts.Domain.Entities.Concerns;

public class ConcernManager(IConcernRepository repository)
    : NamedEntityManager<Concern, IConcernRepository>(repository), IConcernManager;
