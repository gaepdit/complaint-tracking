namespace Cts.Domain.Entities.ActionTypes;

public class ActionTypeManager : NamedEntityManager<ActionType, IActionTypeRepository>, IActionTypeManager
{
    public ActionTypeManager(IActionTypeRepository repository) : base(repository) { }
}
