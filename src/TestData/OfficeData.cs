using Cts.Domain.Entities.Offices;
using Cts.Domain.Identity;

namespace Cts.TestData;

internal static class OfficeData
{
    private static List<Office> OfficeSeedItems => new()
    {
        new Office(new Guid("60000000-0000-0000-0000-000000000001"), "Branch"),
        new Office(new Guid("60000000-0000-0000-0000-000000000002"), "District"),
        new Office(new Guid("60000000-0000-0000-0000-000000000003"), "Region"),
        new Office(new Guid("60000000-0000-0000-0000-000000000004"), "Closed Office") { Active = false },
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
        offices.ElementAt(1).Assignor = users.ElementAt(1);
        offices.ElementAt(2).Assignor = users.ElementAt(2);
        offices.ElementAt(3).Assignor = users.ElementAt(0);
   }
}
