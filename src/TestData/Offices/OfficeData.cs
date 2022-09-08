using Cts.Domain.Entities;
using Cts.TestData.Identity;

namespace Cts.TestData.Offices;

internal static class OfficeData
{
    private static List<Office> OfficeSeedItems(ApplicationUser user) =>
        new()
        {
            new Office(new Guid("00000000-0000-0000-0000-000000000004"), "Air Protection Branch", user)
                { Active = true },
            new Office(new Guid("00000000-0000-0000-0000-000000000005"), "Coastal District", user)
                { Active = true },
            new Office(new Guid("00000000-0000-0000-0000-000000000006"), "Director's Office", user)
                { Active = true },
            // null user in "Air Laboratory (not used)"
            new Office(new Guid("00000000-0000-0000-0000-000000000007"), "Air Laboratory (not used)")
                { Active = false },
        };

    private static ICollection<Office>? _offices;

    public static IEnumerable<Office> GetOffices
    {
        get
        {
            if (_offices is not null) return _offices;

            // Seed offices and user data.
            _offices = OfficeSeedItems(Data.GetUsers.First());
            _offices.First(e => e.Active).StaffMembers = Data.GetUsers.Where(e => e.Active).ToList();
            Data.GetUsers.First().Office = _offices.First();

            return _offices;
        }
    }
}
