using Cts.Domain.Entities;
using Cts.Domain.Users;

namespace Cts.Domain.Offices;

public class OfficeManager : IOfficeManager
{
    private readonly IOfficeRepository _repository;

    public OfficeManager(IOfficeRepository repository) =>
        _repository = repository;

    public async Task<Office> CreateAsync(string name, ApplicationUser? user = null)
    {
        // Validate the name
        var existing = await _repository.FindByNameAsync(name.Trim());
        if (existing is not null) throw new OfficeNameAlreadyExistsException(name);

        return new Office(Guid.NewGuid(), name, user);
    }

    public async Task ChangeNameAsync(Office office, string name)
    {
        var existing = await _repository.FindByNameAsync(name.Trim());
        if (existing is not null && existing.Id == office.Id)
            throw new OfficeNameAlreadyExistsException(name);

        office.ChangeName(name);
    }
}
