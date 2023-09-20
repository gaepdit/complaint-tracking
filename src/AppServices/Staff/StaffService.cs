using AutoMapper;
using Cts.AppServices.Staff.Dto;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities.Offices;
using Cts.Domain.Identity;
using GaEpd.AppLibrary.Domain.Repositories;
using GaEpd.AppLibrary.ListItems;
using GaEpd.AppLibrary.Pagination;
using Microsoft.AspNetCore.Identity;

namespace Cts.AppServices.Staff;

public sealed class StaffService : IStaffService
{
    private readonly IUserService _userService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;
    private readonly IOfficeRepository _officeRepository;

    public StaffService(
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

    public async Task<StaffViewDto?> FindAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        return _mapper.Map<StaffViewDto?>(user);
    }

    public async Task<List<StaffViewDto>> GetListAsync(StaffSearchDto spec)
    {
        var users = string.IsNullOrEmpty(spec.Role)
            ? _userManager.Users.ApplyFilter(spec)
            : (await _userManager.GetUsersInRoleAsync(spec.Role)).AsQueryable().ApplyFilter(spec);

        return _mapper.Map<List<StaffViewDto>>(users);
    }

    public async Task<IPaginatedResult<StaffSearchResultDto>> SearchAsync(StaffSearchDto spec, PaginatedRequest paging)
    {
        var users = string.IsNullOrEmpty(spec.Role)
            ? _userManager.Users.ApplyFilter(spec)
            : (await _userManager.GetUsersInRoleAsync(spec.Role)).AsQueryable().ApplyFilter(spec);
        var list = users.Skip(paging.Skip).Take(paging.Take);
        var listMapped = _mapper.Map<List<StaffSearchResultDto>>(list);

        return new PaginatedResult<StaffSearchResultDto>(listMapped, users.Count(), paging);
    }

    public Task<IReadOnlyList<ListItem<string>>> GetStaffListItemsAsync(bool activeOnly)
    {
        var status = activeOnly ? SearchStaffStatus.Active : SearchStaffStatus.All;
        var spec = new StaffSearchDto(SortBy.NameAsc, null, null, null, null, status);
        var users = _userManager.Users.ApplyFilter(spec);
        return Task.FromResult(_mapper.Map<List<StaffViewDto>>(users)
                .Select(e => new ListItem<string>(e.Id, e.SortableNameWithOffice))
                .ToList()
            as IReadOnlyList<ListItem<string>>);
    }

    public async Task<IList<string>> GetRolesAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null) return new List<string>();
        return await _userManager.GetRolesAsync(user);
    }

    public async Task<IList<AppRole>> GetAppRolesAsync(string id) =>
        AppRole.RolesAsAppRoles(await GetRolesAsync(id)).OrderBy(r => r.DisplayName).ToList();

    public async Task<bool> HasAppRoleAsync(string id, AppRole role)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null) return false;
        return await _userManager.IsInRoleAsync(user, role.Name);
    }

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
        user.Office = resource.OfficeId is null ? null : await _officeRepository.GetAsync(resource.OfficeId.Value);
        user.Active = resource.Active;

        return await _userManager.UpdateAsync(user);
    }

    public void Dispose() => _officeRepository.Dispose();
}
