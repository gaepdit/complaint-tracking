using AutoMapper;
using Cts.AppServices.Users;
using Cts.Domain.Offices;

namespace Cts.AppServices.Offices;

public sealed class OfficeAppService : IOfficeAppService
{
    private readonly IOfficeRepository _repository;
    private readonly IOfficeManager _manager;
    private readonly IMapper _mapper;

    public OfficeAppService(
        IOfficeRepository repository,
        IOfficeManager manager,
        IMapper mapper)
    {
        _repository = repository;
        _manager = manager;
        _mapper = mapper;
    }

    public async Task<OfficeViewDto> GetAsync(Guid id)
    {
        var office = await _repository.GetAsync(id);
        return _mapper.Map<OfficeViewDto>(office);
    }

    public async Task<OfficeUpdateDto?> FindForUpdateAsync(Guid id)
    {
        var office = await _repository.FindAsync(id);
        return _mapper.Map<OfficeUpdateDto>(office);
    }

    public async Task<IReadOnlyList<OfficeViewDto>> GetListAsync()
    {
        var offices = await _repository.GetListAsync();
        return _mapper.Map<IReadOnlyList<OfficeViewDto>>(offices);
    }

    public async Task<OfficeViewDto> CreateAsync(OfficeCreateDto resource)
    {
        // Create and insert the new item
        var office = await _manager.CreateAsync(resource.Name);
        await _repository.InsertAsync(office);

        // Return DTO
        return _mapper.Map<OfficeViewDto>(office);
    }

    public async Task UpdateAsync(OfficeUpdateDto resource)
    {
        var office = await _repository.GetAsync(resource.Id);

        if (office.Name != resource.Name.Trim())
            await _manager.ChangeNameAsync(office, resource.Name);

        office.Active = resource.Active;
        office.MasterUser = resource.MasterUser;

        await _repository.UpdateAsync(office);
    }

    public async Task<IReadOnlyList<UserViewDto>> GetUsersAsync(Guid id)
    {
        var users = await _repository.GetUsersListAsync(id);
        return _mapper.Map<IReadOnlyList<UserViewDto>>(users);
    }

    public void Dispose() => _repository.Dispose();
}
