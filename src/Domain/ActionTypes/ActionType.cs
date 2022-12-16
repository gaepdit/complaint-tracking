using Cts.Domain.BaseEntities;

namespace Cts.Domain.ActionTypes;

public class ActionType : SimpleNamedEntity
{
    public ActionType(Guid id, string name) : base(id, name) { }
}
