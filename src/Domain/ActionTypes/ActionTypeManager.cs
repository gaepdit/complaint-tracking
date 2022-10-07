namespace Cts.Domain.ActionTypes;

/// <inheritdoc />
public class ActionTypeManager : IActionTypeManager
{
    private readonly IActionTypeRepository _repository;
    public ActionTypeManager(IActionTypeRepository repository) => _repository = repository;

    public async Task<ActionType> CreateAsync(string name, CancellationToken token = default)
    {
        await ThrowIfDuplicateName(name, ignoreId: null, token: token);
        return new ActionType(Guid.NewGuid(), name);
    }

    public async Task ChangeNameAsync(ActionType actionType, string name, CancellationToken token = default)
    {
        await ThrowIfDuplicateName(name, actionType.Id, token);
        actionType.ChangeName(name);
    }

    private async Task ThrowIfDuplicateName(string name, Guid? ignoreId, CancellationToken token)
    {
        // Validate the name is not a duplicate
        var existing = await _repository.FindByNameAsync(name.Trim(), token);
        if (existing is not null && (ignoreId is null || existing.Id != ignoreId))
            throw new ActionTypeNameAlreadyExistsException(name);
    }
}
