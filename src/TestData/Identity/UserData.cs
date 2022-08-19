using Cts.Domain.Entities;

namespace Cts.TestData.Identity;

internal static partial class Data
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
            Phone = "123-456-7890",
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
        new ApplicationUser
        {
            Id = Guid.NewGuid().ToString(),
            FirstName = "Inactive",
            LastName = "User",
            Email = "inactive.user@example.net",
            UserName = "inactive.user@example.net",
            NormalizedUserName = "inactive.user@example.net".ToUpperInvariant(),
            Active = false,
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
}
