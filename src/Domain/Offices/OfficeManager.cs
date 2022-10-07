namespace MyAppRoot.Domain.Offices;

/// <inheritdoc />
public class OfficeManager : IOfficeManager
{
    private readonly IOfficeRepository _repository;
    public OfficeManager(IOfficeRepository repository) => _repository = repository;

    public async Task<Office> CreateAsync(string name, CancellationToken token = default)
    {
        await ThrowIfDuplicateName(name, null, token);
        return new Office(Guid.NewGuid(), name);
    }

    public async Task ChangeNameAsync(Office office, string name, CancellationToken token = default)
    {
        await ThrowIfDuplicateName(name, office.Id, token);
        office.ChangeName(name);
    }

    private async Task ThrowIfDuplicateName(string name, Guid? ignoreId, CancellationToken token)
    {
        // Validate the name is not a duplicate
        var existing = await _repository.FindByNameAsync(name.Trim(), token);
        if (existing is not null && (ignoreId is null || existing.Id != ignoreId))
            throw new OfficeNameAlreadyExistsException(name);
    }
}
