using AutoMapper;
using GaEpd.AppLibrary.ListItems;
using MyAppRoot.AppServices.Staff;
using MyAppRoot.AppServices.UserServices;
using MyAppRoot.Domain.Offices;

namespace MyAppRoot.AppServices.Offices;

public sealed class OfficeAppService : IOfficeAppService
{
    private readonly IOfficeRepository _repository;
    private readonly IOfficeManager _manager;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    public OfficeAppService(
        IOfficeRepository repository,
        IOfficeManager manager,
        IMapper mapper,
        IUserService userService)
    {
        _repository = repository;
        _manager = manager;
        _mapper = mapper;
        _userService = userService;
    }

    public async Task<OfficeViewDto?> FindAsync(Guid id, CancellationToken token = default)
    {
        var office = await _repository.FindAsync(id, token);
        return _mapper.Map<OfficeViewDto>(office);
    }

    public async Task<OfficeUpdateDto?> FindForUpdateAsync(Guid id, CancellationToken token = default)
    {
        var office = await _repository.FindAsync(id, token);
        return _mapper.Map<OfficeUpdateDto>(office);
    }

    public async Task<IReadOnlyList<OfficeViewDto>> GetListAsync(CancellationToken token = default)
    {
        var offices = (await _repository.GetListAsync(token)).OrderBy(e => e.Name).ToList();
        return _mapper.Map<IReadOnlyList<OfficeViewDto>>(offices);
    }

    public async Task<IReadOnlyList<ListItem>> GetActiveListItemsAsync(CancellationToken token = default) =>
        (await _repository.GetListAsync(e => e.Active, token)).OrderBy(e => e.Name)
        .Select(e => new ListItem(e.Id, e.Name)).ToList();

    public async Task<Guid> CreateAsync(OfficeCreateDto resource, CancellationToken token = default)
    {
        var office = await _manager.CreateAsync(resource.Name, token);
        office.SetCreator((await _userService.GetCurrentUserAsync())?.Id);
        await _repository.InsertAsync(office, token: token);
        return office.Id;
    }

    public async Task UpdateAsync(OfficeUpdateDto resource, CancellationToken token = default)
    {
        var office = await _repository.GetAsync(resource.Id, token);

        if (office.Name != resource.Name.Trim())
            await _manager.ChangeNameAsync(office, resource.Name, token);
        office.Active = resource.Active;
        office.SetUpdater((await _userService.GetCurrentUserAsync())?.Id);

        await _repository.UpdateAsync(office, token: token);
    }

    public async Task<IReadOnlyList<StaffViewDto>> GetActiveStaffAsync(Guid id, CancellationToken token = default)
    {
        var users = await _repository.GetActiveStaffMembersListAsync(id, token);
        return _mapper.Map<IReadOnlyList<StaffViewDto>>(users);
    }

    public void Dispose() => _repository.Dispose();
}
