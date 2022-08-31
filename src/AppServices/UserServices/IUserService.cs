using Cts.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Cts.AppServices.UserServices;

public interface IUserService
{
    // Current user

    public Task<ApplicationUser?> GetCurrentUserAsync();
    public Task<IList<string>> GetCurrentUserRolesAsync();

    // All users

    public Task<ApplicationUser?> FindUserByIdAsync(string userId);
    public Task<IdentityResult> UpdateUserAsync(ApplicationUser user);

    // Roles

    public Task<IList<string>> GetUserRolesAsync(Guid userId);
    public Task<IList<ApplicationUser>> GetUsersInRoleAsync(string roleName);
    public Task<bool> IsInRoleAsync(ApplicationUser user, string role);
    public Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role);
    public Task<IdentityResult> RemoveFromRoleAsync(ApplicationUser user, string role);
}
