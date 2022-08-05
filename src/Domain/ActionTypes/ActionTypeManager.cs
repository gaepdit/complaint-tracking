namespace Cts.Domain.ActionTypes;

/// <inheritdoc />
public class ActionTypeManager : IActionTypeManager
{
    private readonly IActionTypeRepository _repository;
    public ActionTypeManager(IActionTypeRepository repository) => _repository = repository;

    public async Task<ActionType> CreateAsync(string name)
    {
        // Validate the name
        var existing = await _repository.FindByNameAsync(name.Trim());
        if (existing is not null) throw new ActionTypeNameAlreadyExistsException(name);

        return new ActionType(Guid.NewGuid(), name);
    }

    public async Task ChangeNameAsync(ActionType actionType, string name)
    {
        var existing = await _repository.FindByNameAsync(name.Trim());
        if (existing is not null && existing.Id != actionType.Id)
            throw new ActionTypeNameAlreadyExistsException(name);

        actionType.ChangeName(name);
    }
}
