using Cts.Domain.Identity;

namespace Cts.TestData.Identity;

internal static partial class IdentityData
{
    private static readonly List<ApplicationUser> UserSeedItems = new()
    {
        new ApplicationUser
        {
            Id = "00000000-0000-0000-0000-000000000001",
            FirstName = "Admin",
            LastName = "User",
            Email = "admin.user@example.net",
            Phone = "123-456-7890",
            Office = OfficeData.GetOffices.ElementAt(0),
        },
        new ApplicationUser
        {
            Id = "00000000-0000-0000-0000-000000000002",
            FirstName = "General",
            LastName = "User",
            Email = "general.user@example.net",
            Office = OfficeData.GetOffices.ElementAt(1),
        },
        new ApplicationUser
        {
            Id = "00000000-0000-0000-0000-000000000003",
            FirstName = "Inactive",
            LastName = "User",
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
            UserSeedItems.ForEach(delegate(ApplicationUser user)
            {
                user.UserName = user.Email;
                user.NormalizedEmail = user.Email.ToUpperInvariant();
                user.NormalizedUserName = user.Email.ToUpperInvariant();
            });
            _users = UserSeedItems;
            return _users;
        }
    }
}
