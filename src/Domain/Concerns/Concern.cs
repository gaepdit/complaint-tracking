using Cts.Domain.BaseEntities;

namespace Cts.Domain.Concerns;

public class Concern : SimpleNamedEntity
{
    public Concern(Guid id, string name) : base(id, name) { }
}
