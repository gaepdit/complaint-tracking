using AutoMapper;
using Cts.AppServices.StaffServices;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities;
using Cts.Domain.Identity;
using Cts.Domain.Offices;
using Cts.TestData.Identity;
using GaEpd.Library.Domain.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Cts.LocalRepository.Identity;

public sealed class LocalStaffAppService : IStaffAppService
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    private readonly IdentityErrorDescriber _errorDescriber;
    private readonly IOfficeRepository _officeRepository;

    public LocalStaffAppService(
        IUserService userService,
        IMapper mapper,
        IdentityErrorDescriber errorDescriber,
        IOfficeRepository officeRepository)
    {
        _userService = userService;
        _mapper = mapper;
        _errorDescriber = errorDescriber;
        _officeRepository = officeRepository;
    }

    public Task<StaffViewDto?> FindAsync(Guid id)
    {
        var user = Data.GetUsers.SingleOrDefault(e => e.Id == id.ToString());
        return Task.FromResult(_mapper.Map<StaffViewDto?>(user));
    }

    public async Task<List<StaffViewDto>> GetListAsync(StaffSearchDto filter)
    {
        return string.IsNullOrEmpty(filter.Role)
            ? FilterUsers(Data.GetUsers)
            : FilterUsers(await _userService.GetUsersInRoleAsync(filter.Role));

        List<StaffViewDto> FilterUsers(IEnumerable<ApplicationUser> usersList)
        {
            var users = usersList
                .Where(m => string.IsNullOrEmpty(filter.Name)
                    || m.FirstName.ToLower().Contains(filter.Name.ToLower())
                    || m.LastName.ToLower().Contains(filter.Name.ToLower()))
                .Where(m => string.IsNullOrEmpty(filter.Email)
                    || m.Email == filter.Email)
                .Where(m => filter.Office is null || m.Office?.Id == filter.Office)
                .Where(m => filter.Status == StaffSearchDto.ActiveStatus.All
                    || (filter.Status == StaffSearchDto.ActiveStatus.Active && m.Active)
                    || (filter.Status == StaffSearchDto.ActiveStatus.Inactive && !m.Active)
                )
                .OrderBy(m => m.LastName).ThenBy(m => m.FirstName)
                .ToList();

            return _mapper.Map<List<StaffViewDto>>(users);
        }
    }

    public Task<IList<string>> GetRolesAsync(Guid id) =>
        _userService.GetUserRolesAsync(id);

    public async Task<IList<AppRole>> GetAppRolesAsync(Guid id)
    {
        var roles = await GetRolesAsync(id);
        var appRoles = new List<AppRole>();

        foreach (var role in roles)
            if (AppRole.AllRoles.TryGetValue(role, out var appRole))
                appRoles.Add(appRole);

        return appRoles;
    }

    public async Task<IdentityResult> UpdateRolesAsync(Guid id, Dictionary<string, bool> roles)
    {
        var user = await _userService.FindUserByIdAsync(id.ToString());
        if (user == null) return IdentityResult.Failed(_errorDescriber.DefaultError());

        foreach (var (role, value) in roles)
        {
            var result = await UpdateUserRoleAsync(user, role, value);
            if (result != IdentityResult.Success) return result;
        }

        return IdentityResult.Success;

        async Task<IdentityResult> UpdateUserRoleAsync(ApplicationUser u, string r, bool addToRole)
        {
            var isInRole = await _userService.IsInRoleAsync(u, r);
            if (addToRole == isInRole) return IdentityResult.Success;

            return addToRole switch
            {
                true => await _userService.AddToRoleAsync(u, r),
                false => await _userService.RemoveFromRoleAsync(u, r),
            };
        }
    }

    public async Task<IdentityResult> UpdateAsync(StaffUpdateDto resource)
    {
        var user = await _userService.FindUserByIdAsync(resource.Id.ToString());
        if (user is null) throw new EntityNotFoundException(typeof(ApplicationUser), resource.Id);

        user.Phone = resource.Phone;
        user.Office = await _officeRepository.GetAsync(resource.OfficeId!.Value);
        user.Active = resource.Active;

        return IdentityResult.Success;
    }

    public void Dispose()
    {
        // Method intentionally left empty.
    }
}
