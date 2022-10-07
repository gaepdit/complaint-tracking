using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;

namespace MyAppRoot.TestData.Identity;

internal static partial class IdentityData
{
    private static ICollection<IdentityUserRole<string>>? _userRoles;

    public static IEnumerable<IdentityUserRole<string>> GetUserRoles
    {
        get
        {
            if (_userRoles is null) SeedUserRoles();
            return _userRoles;
        }
    }

    [MemberNotNull(nameof(_userRoles))]
    private static void SeedUserRoles()
    {
        // Add all roles to first user
        var user = GetUsers.First();
        _userRoles = GetIdentityRoles
            .Select(role => new IdentityUserRole<string> { RoleId = role.Id, UserId = user.Id })
            .ToList();
    }

    public static void AddUserRole(this ICollection<IdentityUserRole<string>> userRoles, string userId, string roleName)
    {
        var roleId = GetIdentityRoles.SingleOrDefault(r =>
            string.Equals(r.Name, roleName, StringComparison.InvariantCultureIgnoreCase))?.Id;
        if (roleId is null) return;
        var exists = userRoles.Any(e => e.UserId == userId && e.RoleId == roleId);
        if (!exists) userRoles.Add(new IdentityUserRole<string> { RoleId = roleId, UserId = userId });
    }

    public static void RemoveUserRole(this ICollection<IdentityUserRole<string>> userRoles, string userId,
        string roleName)
    {
        var roleId = GetIdentityRoles.SingleOrDefault(r =>
            string.Equals(r.Name, roleName, StringComparison.InvariantCultureIgnoreCase))?.Id;
        if (roleId is null) return;
        var userRole = userRoles.SingleOrDefault(e => e.UserId == userId && e.RoleId == roleId);
        if (userRole is not null) userRoles.Remove(userRole);
    }
}
