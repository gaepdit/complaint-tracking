using MyAppRoot.Domain.Identity;
using MyAppRoot.TestData.Offices;

namespace MyAppRoot.TestData.Identity;

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
        },
        new ApplicationUser
        {
            Id = "00000000-0000-0000-0000-000000000002",
            FirstName = "General",
            LastName = "User",
            Email = "general.user@example.net",
        },
        new ApplicationUser
        {
            Id = "00000000-0000-0000-0000-000000000003",
            FirstName = "Inactive",
            LastName = "User",
            Email = "inactive.user@example.net",
            Active = false,
        },
    };

    private static IEnumerable<ApplicationUser>? _users;

    public static IEnumerable<ApplicationUser> GetUsers
    {
        get
        {
            if (_users is not null) return _users;
            var office = OfficeData.GetOffices.First();
            UserSeedItems.ForEach(delegate(ApplicationUser user)
            {
                user.Office = office;
                user.UserName = user.Email;
                user.NormalizedEmail = user.Email.ToUpperInvariant();
                user.NormalizedUserName = user.Email.ToUpperInvariant();
            });
            _users = UserSeedItems;
            return _users;
        }
    }
}
