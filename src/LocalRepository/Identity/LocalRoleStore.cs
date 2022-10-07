using Microsoft.AspNetCore.Identity;
using MyAppRoot.TestData.Identity;

namespace MyAppRoot.LocalRepository.Identity;

/// <summary>
/// This store is intentionally only partially implemented. RoleStore is read-only.
/// </summary>
public sealed class LocalRoleStore : IRoleStore<IdentityRole>
{
    public Task<IdentityResult> CreateAsync(IdentityRole role, CancellationToken cancellationToken) =>
        Task.FromResult(new IdentityResult()); // Intentionally left unimplemented.

    public Task<IdentityResult> UpdateAsync(IdentityRole role, CancellationToken cancellationToken) =>
        Task.FromResult(new IdentityResult()); // Intentionally left unimplemented.

    public Task<IdentityResult> DeleteAsync(IdentityRole role, CancellationToken cancellationToken) =>
        Task.FromResult(new IdentityResult()); // Intentionally left unimplemented.

    public Task<string> GetRoleIdAsync(IdentityRole role, CancellationToken cancellationToken) =>
        Task.FromResult(role.Id);

    public Task<string> GetRoleNameAsync(IdentityRole role, CancellationToken cancellationToken) =>
        Task.FromResult(role.Name);

    public Task SetRoleNameAsync(IdentityRole role, string roleName, CancellationToken cancellationToken) =>
        Task.CompletedTask; // Intentionally left unimplemented.

    public Task<string> GetNormalizedRoleNameAsync(IdentityRole role, CancellationToken cancellationToken) =>
        Task.FromResult(role.NormalizedName);

    public Task SetNormalizedRoleNameAsync(IdentityRole role, string normalizedName,
        CancellationToken cancellationToken) =>
        Task.CompletedTask; // Intentionally left unimplemented.

    public Task<IdentityRole> FindByIdAsync(string roleId, CancellationToken cancellationToken) =>
        Task.FromResult(IdentityData.GetIdentityRoles.Single(r => r.Id == roleId));

    public Task<IdentityRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken) =>
        Task.FromResult(IdentityData.GetIdentityRoles.Single(r =>
            string.Equals(r.NormalizedName, normalizedRoleName, StringComparison.InvariantCultureIgnoreCase)));

    public void Dispose()
    {
        // Method intentionally left empty.
    }
}
