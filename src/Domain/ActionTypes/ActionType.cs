using Cts.Domain.BaseEntities;

namespace Cts.Domain.ActionTypes;

public class ActionType : SimpleNamedEntity
{
    internal ActionType(Guid id, string name) : base(id, name) { }
}
