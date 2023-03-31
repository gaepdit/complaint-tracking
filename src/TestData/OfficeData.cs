using Cts.Domain.Entities.Offices;
using Cts.Domain.Identity;

namespace Cts.TestData;

internal static class OfficeData
{
    private static List<Office> OfficeSeedItems => new()
    {
        new Office(new Guid("00000000-0000-0000-0000-000000000004"), "Branch"),
        new Office(new Guid("00000000-0000-0000-0000-000000000005"), "District"),
        new Office(new Guid("00000000-0000-0000-0000-000000000006"), "Region"),
        new Office(new Guid("00000000-0000-0000-0000-000000000007"), "Closed Office") { Active = false },
    };

    private static ICollection<Office>? _offices;

    public static ICollection<Office> GetOffices
    {
        get
        {
            if (_offices is not null) return _offices;
            _offices = OfficeSeedItems;
            return _offices;
        }
    }

    public static void ClearData() => _offices = null;

    internal static void SeedOfficeAssignors(ICollection<Office> offices, ICollection<ApplicationUser> users)
    {
        offices.ElementAt(0).Assignor = users.ElementAt(0);
        offices.ElementAt(0).AssignorId = users.ElementAt(0).Id;
        offices.ElementAt(1).Assignor = users.ElementAt(1);
        offices.ElementAt(1).AssignorId = users.ElementAt(1).Id;
        offices.ElementAt(2).Assignor = users.ElementAt(2);
        offices.ElementAt(2).AssignorId = users.ElementAt(2).Id;
        offices.ElementAt(3).Assignor = users.ElementAt(0);
        offices.ElementAt(3).AssignorId = users.ElementAt(0).Id;
    }
}
