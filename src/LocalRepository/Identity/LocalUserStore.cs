using Cts.Domain.Identity;
using Cts.TestData.Identity;
using Microsoft.AspNetCore.Identity;

namespace Cts.LocalRepository.Identity;

/// <summary>
/// This store is only partially implemented. UserStore is read-only, except Update. UserRoleStore is read/write.
/// </summary>
public sealed class LocalUserStore : IUserRoleStore<ApplicationUser> // inherits IUserStore<ApplicationUser>
{
    internal ICollection<ApplicationUser> Users { get; }
    internal ICollection<IdentityRole> Roles { get; }
    private ICollection<IdentityUserRole<string>> UserRoles { get; }

    public LocalUserStore()
    {
        Users = IdentityData.GetUsers.ToList();
        Roles = IdentityData.GetRoles.ToList();
        UserRoles = Roles
            .Select(role => new IdentityUserRole<string> { RoleId = role.Id, UserId = Users.First().Id })
            .ToList();
    }

    // IUserStore
    public Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken) =>
        Task.FromResult(user.Id);

    public Task<string> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken) =>
        Task.FromResult(user.UserName);

    public Task SetUserNameAsync(ApplicationUser user, string userName, CancellationToken cancellationToken) =>
        Task.CompletedTask; // Intentionally left unimplemented.

    public Task<string> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken) =>
        Task.FromResult(user.NormalizedUserName);

    public Task SetNormalizedUserNameAsync(ApplicationUser user, string normalizedName,
        CancellationToken cancellationToken) =>
        Task.CompletedTask;

    public Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken) =>
        Task.FromResult(new IdentityResult()); // Intentionally left unimplemented.

    public async Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        var existingUser = await FindByIdAsync(user.Id, cancellationToken);
        Users.Remove(existingUser);
        Users.Add(user);
        return IdentityResult.Success;
    }

    public Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken) =>
        Task.FromResult(new IdentityResult()); // Intentionally left unimplemented.

    public Task<ApplicationUser> FindByIdAsync(string userId, CancellationToken cancellationToken) =>
        Task.FromResult(Users.Single(u => u.Id == userId));

    public Task<ApplicationUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken) =>
        Task.FromResult(Users.Single(u =>
            string.Equals(u.NormalizedUserName, normalizedUserName, StringComparison.InvariantCultureIgnoreCase)));

    // IUserRoleStore
    public Task AddToRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
    {
        var roleId = Roles.SingleOrDefault(r =>
            string.Equals(r.Name, roleName, StringComparison.InvariantCultureIgnoreCase))?.Id;
        if (roleId is null) return Task.CompletedTask;

        var exists = UserRoles.Any(e => e.UserId == user.Id && e.RoleId == roleId);
        if (!exists) UserRoles.Add(new IdentityUserRole<string> { RoleId = roleId, UserId = user.Id });

        return Task.CompletedTask;
    }

    public Task RemoveFromRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
    {
        var roleId = Roles.SingleOrDefault(r =>
            string.Equals(r.Name, roleName, StringComparison.InvariantCultureIgnoreCase))?.Id;
        if (roleId is null) return Task.CompletedTask;

        var userRole = UserRoles.SingleOrDefault(e => e.UserId == user.Id && e.RoleId == roleId);
        if (userRole is not null) UserRoles.Remove(userRole);

        return Task.CompletedTask;
    }

    public Task<IList<string>> GetRolesAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        var roleIdsForUser = UserRoles
            .Where(e => e.UserId == user.Id)
            .Select(e => e.RoleId);
        var rolesForUser = Roles
            .Where(r => roleIdsForUser.Contains(r.Id))
            .Select(r => r.NormalizedName).ToList();
        return Task.FromResult<IList<string>>(rolesForUser);
    }

    public Task<bool> IsInRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
    {
        var roleId = Roles.SingleOrDefault(r =>
            string.Equals(r.Name, roleName, StringComparison.InvariantCultureIgnoreCase))?.Id;
        return Task.FromResult(UserRoles.Any(e => e.UserId == user.Id && e.RoleId == roleId));
    }

    public Task<IList<ApplicationUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
    {
        var roleId = Roles.SingleOrDefault(r =>
            string.Equals(r.Name, roleName, StringComparison.InvariantCultureIgnoreCase))?.Id;
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
