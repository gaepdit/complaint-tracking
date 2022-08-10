using Cts.Domain.Users;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;

namespace Cts.TestData.Identity;

internal static class Data
{
    private static readonly List<ApplicationUser> UserSeedItems = new()
    {
        new ApplicationUser
        {
            Id = Guid.NewGuid().ToString(),
            FirstName = "Local",
            LastName = "User",
            Email = "local.user@example.net",
            UserName = "local.user@example.net",
            NormalizedUserName = "local.user@example.net".ToUpperInvariant(),
        },
        new ApplicationUser
        {
            Id = Guid.NewGuid().ToString(),
            FirstName = "Another",
            LastName = "User",
            Email = "another.user@example.net",
            UserName = "another.user@example.net",
            NormalizedUserName = "another.user@example.net".ToUpperInvariant(),
        },
    };

    private static IEnumerable<ApplicationUser>? _users;

    public static IEnumerable<ApplicationUser> GetUsers
    {
        get
        {
            if (_users is not null) return _users;
            _users = UserSeedItems;
            return _users;
        }
    }

    private static IEnumerable<IdentityRole>? _roles;

    public static IEnumerable<IdentityRole> GetRoles
    {
        get
        {
            if (_roles is not null) return _roles;
            _roles = UserRole.AllRoles.Select(r =>
                    new IdentityRole(r.Key) { NormalizedName = r.Key.ToUpperInvariant() })
                .ToList();
            return _roles;
        }
    }

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
        _userRoles = GetRoles
            .Select(role => new IdentityUserRole<string> { RoleId = role.Id, UserId = user.Id, })
            .ToList();
    }

    public static void AddUserRole(this ICollection<IdentityUserRole<string>> userRoles, string userId, string roleName)
    {
        var roleId = GetRoles.SingleOrDefault(r => r.NormalizedName == roleName.ToUpperInvariant())?.Id;
        if (roleId is null) return;
        var exists = userRoles.Any(e => e.UserId == userId && e.RoleId == roleId);
        if (!exists) userRoles.Add(new IdentityUserRole<string> { RoleId = roleId, UserId = userId });
    }

    public static void RemoveUserRole(this ICollection<IdentityUserRole<string>> userRoles, string userId,
        string roleName)
    {
        var roleId = GetRoles.SingleOrDefault(r => r.NormalizedName == roleName.ToUpperInvariant())?.Id;
        if (roleId is null) return;
        var userRole = userRoles.SingleOrDefault(e => e.UserId == userId && e.RoleId == roleId);
        if (userRole is not null) userRoles.Remove(userRole);
    }
}
