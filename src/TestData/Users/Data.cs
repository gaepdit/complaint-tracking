using Cts.Domain.Users;

namespace Cts.TestData.Users;

internal static class Data
{
    public static readonly List<ApplicationUser> UsersList = new()
    {
        new ApplicationUser
        {
            Id = Guid.NewGuid().ToString(),
            FirstName = "Local",
            LastName = "User",
            Email = "local.user@example.net",
        },
    };
}
