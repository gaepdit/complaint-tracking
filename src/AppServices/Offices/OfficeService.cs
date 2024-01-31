using AutoMapper;
using Cts.AppServices.ServiceBase;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities.Offices;
using GaEpd.AppLibrary.ListItems;

namespace Cts.AppServices.Offices;

public sealed class OfficeService(
    IOfficeRepository repository,
    IOfficeManager manager,
    IMapper mapper,
    IUserService userService)
    : MaintenanceItemService<Office, OfficeWithAssignorDto, OfficeUpdateDto>
        (repository, manager, mapper, userService),
        IOfficeService
{
    private readonly IMapper _mapper = mapper;
    private readonly IUserService _userService = userService;

    public async Task<OfficeWithAssignorDto?> FindAsync(Guid id, CancellationToken token = default)
    {
        var item = await repository.FindIncludeAssignorAsync(id, token);
        return _mapper.Map<OfficeWithAssignorDto>(item);
    }

    public async Task<Guid> CreateAsync(OfficeCreateDto resource, CancellationToken token = default)
    {
        var item = await manager.CreateAsync(resource.Name, (await _userService.GetCurrentUserAsync())?.Id, token);
        if (resource.AssignorId != null) item.Assignor = await _userService.FindUserAsync(resource.AssignorId);
        await repository.InsertAsync(item, token: token);
        return item.Id;
    }

    public async Task<IReadOnlyList<ListItem<string>>> GetStaffListItemsAsync(Guid? id, bool includeInactive = false,
        CancellationToken token = default) =>
        id is null
            ? Array.Empty<ListItem<string>>()
            : (await repository.GetStaffMembersListAsync(id.Value, includeInactive, token))
            .Select(e => new ListItem<string>(e.Id, e.SortableNameWithInactive))
            .ToList();

    public async Task<bool> UserIsAssignorAsync(Guid id, string userId, CancellationToken token = default)
    {
        var office = await repository.FindIncludeAssignorAsync(id, token);
        return office is { Active: true } && office.Assignor?.Id == userId;
    }
}
