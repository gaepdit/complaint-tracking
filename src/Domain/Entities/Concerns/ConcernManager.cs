namespace Cts.Domain.Entities.Concerns;

public class ConcernManager : NamedEntityManager<Concern, IConcernRepository>, IConcernManager
{
    public ConcernManager(IConcernRepository repository) : base(repository) { }
}
