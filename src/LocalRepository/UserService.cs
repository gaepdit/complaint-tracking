using AutoMapper;
using Cts.AppServices.Users;
using Cts.Domain.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using static Cts.TestData.Users.Data;

namespace Cts.LocalRepository;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;

    public UserService(
        UserManager<ApplicationUser> userManager,
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper)
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
    }

    private async Task<ApplicationUser?> GetCurrentApplicationUserAsync()
    {
        var principal = _httpContextAccessor.HttpContext?.User;
        return principal == null ? null : await _userManager.GetUserAsync(principal);
    }

    public async Task<UserViewDto?> GetCurrentUserAsync(CancellationToken token = default)
    {
        var user = await GetCurrentApplicationUserAsync();
        return _mapper.Map<UserViewDto?>(user);
    }

    public async Task<IList<string>> GetCurrentUserRolesAsync(CancellationToken token = default)
    {
        var user = await GetCurrentApplicationUserAsync();
        return user is null ? new List<string>() : await _userManager.GetRolesAsync(user);
    }

    private List<UserViewDto> FilterUsers(
        IEnumerable<ApplicationUser> usersList,
        string? nameFilter,
        string? emailFilter)
    {
        var users = usersList
            .Where(m => string.IsNullOrEmpty(nameFilter)
                || m.FirstName.Contains(nameFilter)
                || m.LastName.Contains(nameFilter))
            .Where(m => string.IsNullOrEmpty(emailFilter)
                || m.Email == emailFilter)
            .OrderBy(m => m.LastName).ThenBy(m => m.FirstName)
            .ToList();

        return _mapper.Map<List<UserViewDto>>(users);
    }

    public async Task<List<UserViewDto>> FindUsersAsync(
        string? nameFilter,
        string? emailFilter,
        string? role,
        CancellationToken token = default) =>
        string.IsNullOrEmpty(role)
            ? FilterUsers(UsersList, nameFilter, emailFilter)
            : FilterUsers(await _userManager.GetUsersInRoleAsync(role), nameFilter, emailFilter);

    public Task<UserViewDto?> GetUserByIdAsync(string id, CancellationToken token = default) =>
        Task.FromResult(_mapper.Map<UserViewDto?>(UsersList.SingleOrDefault(e => e.Id == id)));

    public async Task<IList<string>> GetUserRolesAsync(string id, CancellationToken token = default)
    {
        var user = await _userManager.FindByIdAsync(id);
        return user == null ? new List<string>() : await _userManager.GetRolesAsync(user);
    }

    public async Task<IdentityResult> UpdateUserRolesAsync(
        string id,
        Dictionary<string, bool> roleUpdates,
        CancellationToken token = default)
    {
        foreach (var (key, value) in roleUpdates)
        {
            var result = await UpdateUserRoleAsync(id, key, value);
            if (result != IdentityResult.Success) return result;
        }

        return IdentityResult.Success;
    }

    private async Task<IdentityResult> UpdateUserRoleAsync(string id, string role, bool addToRole)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return IdentityResult.Failed(_userManager.ErrorDescriber.DefaultError());

        var isInRole = await _userManager.IsInRoleAsync(user, role);
        if (addToRole == isInRole) return IdentityResult.Success;

        return addToRole switch
        {
            true => await _userManager.AddToRoleAsync(user, role),
            false => await _userManager.RemoveFromRoleAsync(user, role),
        };
    }
}
