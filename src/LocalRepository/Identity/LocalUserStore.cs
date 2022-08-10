using Cts.Domain.Users;
using Cts.TestData.Identity;
using Microsoft.AspNetCore.Identity;

namespace Cts.LocalRepository.Identity;

/// <summary>
/// This store is only partially implemented. UserStore is read-only. UserRoleStore is read/write.
/// </summary>
public sealed class LocalUserStore : IUserRoleStore<ApplicationUser> // inherits IUserStore<ApplicationUser>
{
    internal ICollection<ApplicationUser> Users { get; }
    internal ICollection<IdentityUserRole<string>> UserRoles { get; }

    public LocalUserStore()
    {
        Users = Data.GetUsers.ToList();
        UserRoles = Data.GetUserRoles.ToList();
    }

    // IUserStore
    public Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken) =>
        Task.FromResult(user.Id);

    public Task<string> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken) =>
        Task.FromResult(user.UserName);

    public Task SetUserNameAsync(ApplicationUser user, string userName, CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    public Task<string> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken) =>
        Task.FromResult(user.NormalizedUserName);

    public Task SetNormalizedUserNameAsync(ApplicationUser user, string normalizedName,
        CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    public Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    public Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    public Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    public Task<ApplicationUser> FindByIdAsync(string userId, CancellationToken cancellationToken) =>
        Task.FromResult(Users.Single(u => u.Id == userId));

    public Task<ApplicationUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken) =>
        Task.FromResult(Users.Single(u => u.NormalizedUserName == normalizedUserName));

    // IUserRoleStore
    public Task AddToRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
    {
        UserRoles.AddUserRole(user.Id, roleName);
        return Task.CompletedTask;
    }

    public Task RemoveFromRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
    {
        UserRoles.RemoveUserRole(user.Id, roleName);
        return Task.CompletedTask;
    }

    public Task<IList<string>> GetRolesAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        var roleIdsForUser = UserRoles
            .Where(e => e.UserId == user.Id)
            .Select(e => e.RoleId);
        var rolesForUser = Data.GetRoles
            .Where(r => roleIdsForUser.Contains(r.Id))
            .Select(r => r.Name).ToList();
        return Task.FromResult<IList<string>>(rolesForUser);
    }

    public Task<bool> IsInRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
    {
        var roleId = Data.GetRoles.SingleOrDefault(r => r.NormalizedName == roleName.ToUpperInvariant())?.Id;
        return Task.FromResult(UserRoles.Any(e => e.UserId == user.Id && e.RoleId == roleId));
    }

    public Task<IList<ApplicationUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
    {
        var roleId = Data.GetRoles.SingleOrDefault(r => r.NormalizedName == roleName.ToUpperInvariant())?.Id;
        var userIdsInRole = UserRoles
            .Where(e => e.RoleId == roleId)
            .Select(e => e.UserId);
        var usersInRole = Users
            .Where(u => userIdsInRole.Contains(u.Id)).ToList();
        return Task.FromResult<IList<ApplicationUser>>(usersInRole);
    }

    public void Dispose()
    {
        // Method intentionally left empty.
    }
}
