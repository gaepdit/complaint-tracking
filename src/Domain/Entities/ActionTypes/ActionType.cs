using Cts.Domain.Entities.EntityBase;

namespace Cts.Domain.Entities.ActionTypes;

public class ActionType : SimpleNamedEntity
{
    internal ActionType(Guid id, string name) : base(id, name) { }
}
