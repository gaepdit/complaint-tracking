using Cts.Domain.Identity;

namespace Cts.TestData.Identity;

internal static partial class UserData
{
    private static List<ApplicationUser> UserSeedItems => new()
    {
        new ApplicationUser
        {
            Id = "00000000-0000-0000-0000-000000000001",
            GivenName = "Admin",
            FamilyName = "User",
            Email = "admin.user@example.net",
            Phone = "123-456-7890",
            Office = OfficeData.GetOffices.ElementAt(0),
        },
        new ApplicationUser
        {
            Id = "00000000-0000-0000-0000-000000000002",
            GivenName = "General",
            FamilyName = "User",
            Email = "general.user@example.net",
            Office = OfficeData.GetOffices.ElementAt(1),
        },
        new ApplicationUser
        {
            Id = "00000000-0000-0000-0000-000000000003",
            GivenName = "Inactive",
            FamilyName = "User",
            Email = "inactive.user@example.net",
            Active = false,
            Office = OfficeData.GetOffices.ElementAt(1),
        },
    };

    private static IEnumerable<ApplicationUser>? _users;

    public static IEnumerable<ApplicationUser> GetUsers
    {
        get
        {
            if (_users is not null) return _users;
            _users = UserSeedItems;

            foreach (var user in _users)
            {
                user.UserName = user.Email?.ToLowerInvariant();
                user.NormalizedEmail = user.Email?.ToUpperInvariant();
                user.NormalizedUserName = user.Email?.ToUpperInvariant();
            }

            return _users;
        }
    }

    public static void ClearData() => _users = null;
}
