using Cts.TestData.Identity;
using Microsoft.AspNetCore.Identity;

namespace Cts.LocalRepository.Identity;

/// <summary>
/// This store is only partially implemented. RoleStore is read-only.
/// </summary>
public sealed class LocalRoleStore : IRoleStore<IdentityRole>
{
    public Task<IdentityResult> CreateAsync(IdentityRole role, CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    public Task<IdentityResult> UpdateAsync(IdentityRole role, CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    public Task<IdentityResult> DeleteAsync(IdentityRole role, CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    public Task<string> GetRoleIdAsync(IdentityRole role, CancellationToken cancellationToken) =>
        Task.FromResult(role.Id);

    public Task<string> GetRoleNameAsync(IdentityRole role, CancellationToken cancellationToken) =>
        Task.FromResult(role.Name);

    public Task SetRoleNameAsync(IdentityRole role, string roleName, CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    public Task<string> GetNormalizedRoleNameAsync(IdentityRole role, CancellationToken cancellationToken) =>
        Task.FromResult(role.NormalizedName);

    public Task SetNormalizedRoleNameAsync(IdentityRole role, string normalizedName,
        CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    public Task<IdentityRole> FindByIdAsync(string roleId, CancellationToken cancellationToken) =>
        Task.FromResult(Data.GetIdentityRoles.Single(r => r.Id == roleId));

    public Task<IdentityRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken) =>
        Task.FromResult(Data.GetIdentityRoles.Single(r => r.NormalizedName == normalizedRoleName));

    public void Dispose()
    {
        // Method intentionally left empty.
    }
}
