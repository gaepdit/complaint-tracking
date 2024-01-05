namespace Cts.Domain.Entities.ActionTypes;

public class ActionTypeManager(IActionTypeRepository repository)
    : NamedEntityManager<ActionType, IActionTypeRepository>(repository), IActionTypeManager;
