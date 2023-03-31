using AutoMapper;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities.Offices;
using Cts.Domain.Identity;
using GaEpd.AppLibrary.Domain.Repositories;
using GaEpd.AppLibrary.ListItems;
using Microsoft.AspNetCore.Identity;

namespace Cts.AppServices.Staff;

public sealed class StaffAppService : IStaffAppService
{
    private readonly IUserService _userService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;
    private readonly IOfficeRepository _officeRepository;

    public StaffAppService(
        IUserService userService,
        UserManager<ApplicationUser> userManager,
        IMapper mapper,
        IOfficeRepository officeRepository)
    {
        _userService = userService;
        _userManager = userManager;
        _mapper = mapper;
        _officeRepository = officeRepository;
    }

    public async Task<StaffViewDto> GetCurrentUserAsync()
    {
        var user = await _userService.GetCurrentUserAsync()
            ?? throw new CurrentUserNotFoundException();
        return _mapper.Map<StaffViewDto>(user);
    }

    public async Task<StaffViewDto> GetAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id)
            ?? throw new EntityNotFoundException(typeof(ApplicationUser), id);
        return _mapper.Map<StaffViewDto>(user);
    }

    public async Task<StaffViewDto?> FindAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        return _mapper.Map<StaffViewDto?>(user);
    }

    public async Task<List<StaffViewDto>> GetListAsync(StaffSearchDto filter)
    {
        var users = string.IsNullOrEmpty(filter.Role)
            ? _userManager.Users.ApplyFilter(filter)
            : (await _userManager.GetUsersInRoleAsync(filter.Role)).AsQueryable().ApplyFilter(filter);

        return _mapper.Map<List<StaffViewDto>>(users);
    }

    public async Task<IReadOnlyList<ListItem<string>>> GetActiveStaffMembersAsync() =>
        (await GetListAsync(new StaffSearchDto { Status = StaffSearchDto.ActiveStatus.Active }))
            .Select(e => new ListItem<string>(e.Id, e.SelectableNameWithOffice)).ToList();

    public async Task<IReadOnlyList<ListItem<string>>> GetAllStaffMembersAsync() =>
        (await GetListAsync(new StaffSearchDto { Status = StaffSearchDto.ActiveStatus.All }))
            .Select(e => new ListItem<string>(e.Id, e.SelectableNameWithOffice)).ToList();

    public async Task<IList<string>> GetRolesAsync(string id)
    {
        ApplicationUser? user = await _userManager.FindByIdAsync(id);
        if (user is null) return new List<string>();
        return await _userManager.GetRolesAsync(user);
    }

    public async Task<IList<AppRole>> GetAppRolesAsync(string id) => AppRole.RolesAsAppRoles(await GetRolesAsync(id));

    public async Task<IdentityResult> UpdateRolesAsync(string id, Dictionary<string, bool> roles)
    {
        var user = await _userManager.FindByIdAsync(id)
            ?? throw new EntityNotFoundException(typeof(ApplicationUser), id);

        foreach (var (role, value) in roles)
        {
            var result = await UpdateUserRoleAsync(user, role, value);
            if (result != IdentityResult.Success) return result;
        }

        return IdentityResult.Success;

        async Task<IdentityResult> UpdateUserRoleAsync(ApplicationUser u, string r, bool addToRole)
        {
            var isInRole = await _userManager.IsInRoleAsync(u, r);
            if (addToRole == isInRole) return IdentityResult.Success;

            return addToRole switch
            {
                true => await _userManager.AddToRoleAsync(u, r),
                false => await _userManager.RemoveFromRoleAsync(u, r),
            };
        }
    }

    public async Task<IdentityResult> UpdateAsync(StaffUpdateDto resource)
    {
        var user = await _userManager.FindByIdAsync(resource.Id)
            ?? throw new EntityNotFoundException(typeof(ApplicationUser), resource.Id);

        user.Phone = resource.Phone;
        user.Office = resource.OfficeId is null ? null : await _officeRepository.FindAsync(resource.OfficeId.Value);
        user.Active = resource.Active;

        return await _userManager.UpdateAsync(user);
    }

    public void Dispose() => _officeRepository.Dispose();
}
