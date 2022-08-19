using Cts.AppServices.UserServices;
using Cts.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Cts.LocalRepository.Identity;

public class LocalUserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LocalUserService(
        UserManager<ApplicationUser> userManager,
        IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ApplicationUser?> GetCurrentUserAsync()
    {
        var principal = _httpContextAccessor.HttpContext?.User;
        return principal == null ? null : await _userManager.GetUserAsync(principal);
    }

    public async Task<IList<string>> GetCurrentUserRolesAsync()
    {
        var user = await GetCurrentUserAsync();
        return user is null ? new List<string>() : await _userManager.GetRolesAsync(user);
    }

    public async Task<ApplicationUser?> FindUserByIdAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        return user;
    }

    public Task<IdentityResult> UpdateUserAsync(ApplicationUser user) =>
        _userManager.UpdateAsync(user);

    public async Task<IList<string>> GetUserRolesAsync(string userId) =>
        await _userManager.GetRolesAsync(await _userManager.FindByIdAsync(userId));

    public Task<IList<ApplicationUser>> GetUsersInRoleAsync(string roleName) =>
        _userManager.GetUsersInRoleAsync(roleName);

    public Task<bool> IsInRoleAsync(ApplicationUser user, string role) =>
        _userManager.IsInRoleAsync(user, role);

    public Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role) =>
        _userManager.AddToRoleAsync(user, role);

    public Task<IdentityResult> RemoveFromRoleAsync(ApplicationUser user, string role) =>
        _userManager.RemoveFromRoleAsync(user, role);
}
