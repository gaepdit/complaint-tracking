using Cts.Domain.Entities.BaseEntities;

namespace Cts.Domain.Entities.Concerns;

public class Concern : SimpleNamedEntity
{
    internal Concern(Guid id, string name) : base(id, name) { }
}
