using AutoMapper;
using Cts.AppServices.StaffServices;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities;
using Cts.Domain.Identity;
using Cts.TestData.Identity;
using GaEpd.Library.Domain.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Cts.LocalRepository.Identity;

public sealed class LocalStaffAppService : IStaffAppService
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    private readonly IdentityErrorDescriber _errorDescriber;

    public LocalStaffAppService(IUserService userService, IMapper mapper, IdentityErrorDescriber errorDescriber)
    {
        _userService = userService;
        _mapper = mapper;
        _errorDescriber = errorDescriber;
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

    public async Task<IList<CtsRole>> GetCtsRolesAsync(Guid id)
    {
        var roles = await GetRolesAsync(id);
        var ctsRoles = new List<CtsRole>();

        foreach (var role in roles)
        {
            if (CtsRole.AllRoles.TryGetValue(role.ToUpperInvariant(), out var ctsRole))
            {
                ctsRoles.Add(ctsRole);
            }
        }

        return ctsRoles;
    }

    public async Task<IdentityResult> UpdateRolesAsync(Guid id, Dictionary<CtsRole, bool> roles)
    {
        var user = await _userService.FindUserByIdAsync(id.ToString());
        if (user == null) return IdentityResult.Failed(_errorDescriber.DefaultError());

        foreach (var (role, value) in roles)
        {
            var result = await UpdateUserRoleAsync(user, role.Name, value);
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
        user.Office = resource.Office;
        user.Active = resource.Active;

        return await _userService.UpdateUserAsync(user);
    }

    public void Dispose()
    {
        // Method intentionally left empty.
    }
}
