using Cts.Domain.Identity;

namespace Cts.TestData.Identity;

internal static partial class UserData
{
    private static IEnumerable<ApplicationUser> UserSeedItems => new List<ApplicationUser>
    {
        new()
        {
            Id = "00000000-0000-0000-0000-000000000001",
            GivenName = "Admin",
            FamilyName = "User1",
            Email = "admin.user@example.net",
            Phone = "123-456-7890",
            Office = OfficeData.GetOffices.ElementAt(0),
        },
        new()
        {
            Id = "00000000-0000-0000-0000-000000000002",
            GivenName = "General",
            FamilyName = "User2",
            Email = "general.user@example.net",
            Office = OfficeData.GetOffices.ElementAt(1),
        },
        new()
        {
            Id = "00000000-0000-0000-0000-000000000003",
            GivenName = "Limited",
            FamilyName = "User3",
            Email = "limited.user@example.net",
            Office = OfficeData.GetOffices.ElementAt(1),
        },
        new()
        {
            Id = "00000000-0000-0000-0000-000000000004",
            GivenName = "Inactive",
            FamilyName = "User4",
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
            _users = UserSeedItems.ToList();

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
