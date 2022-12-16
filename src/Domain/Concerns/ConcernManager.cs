namespace Cts.Domain.Concerns;

/// <inheritdoc />
public class ConcernManager : IConcernManager
{
    private readonly IConcernRepository _repository;
    public ConcernManager(IConcernRepository repository) => _repository = repository;

    public async Task<Concern> CreateAsync(string name, CancellationToken token = default)
    {
        await ThrowIfDuplicateName(name, ignoreId: null, token: token);
        return new Concern(Guid.NewGuid(), name);
    }

    public async Task ChangeNameAsync(Concern concern, string name, CancellationToken token = default)
    {
        await ThrowIfDuplicateName(name, concern.Id, token);
        concern.ChangeName(name);
    }

    private async Task ThrowIfDuplicateName(string name, Guid? ignoreId, CancellationToken token)
    {
        // Validate the name is not a duplicate
        var existing = await _repository.FindByNameAsync(name.Trim(), token);
        if (existing is not null && (ignoreId is null || existing.Id != ignoreId))
            throw new ConcernNameAlreadyExistsException(name);
    }
}
