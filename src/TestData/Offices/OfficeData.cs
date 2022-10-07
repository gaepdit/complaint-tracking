using MyAppRoot.Domain.Offices;

namespace MyAppRoot.TestData.Offices;

internal static class OfficeData
{
    private static readonly List<Office> OfficeSeedItems = new()
    {
        new Office(new Guid("00000000-0000-0000-0000-000000000004"), "Branch"),
        new Office(new Guid("00000000-0000-0000-0000-000000000005"), "District"),
        new Office(new Guid("00000000-0000-0000-0000-000000000006"), "Region"),
        new Office(new Guid("00000000-0000-0000-0000-000000000007"), "Closed Office") { Active = false },
    };

    private static ICollection<Office>? _offices;

    public static IEnumerable<Office> GetOffices
    {
        get
        {
            if (_offices is not null) return _offices;
            _offices = OfficeSeedItems;
            return _offices;
        }
    }
}
