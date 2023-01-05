using Cts.Domain.BaseEntities;

namespace Cts.Domain.Concerns;

public class Concern : SimpleNamedEntity
{
    internal Concern(Guid id, string name) : base(id, name) { }
}
