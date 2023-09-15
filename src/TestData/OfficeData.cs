using Cts.Domain.Entities.Offices;

namespace Cts.TestData;

internal static class OfficeData
{
    private static IEnumerable<Office> OfficeSeedItems => new List<Office>
    {
        new(new Guid("00000000-0000-0000-0000-000000000004"), "Branch"),
        new(new Guid("00000000-0000-0000-0000-000000000005"), "District"),
        new(new Guid("00000000-0000-0000-0000-000000000006"), "Region"),
        new(new Guid("00000000-0000-0000-0000-000000000007"), "Closed Office") { Active = false },
    };

    private static IEnumerable<Office>? _offices;

    public static IEnumerable<Office> GetOffices
    {
        get
        {
            if (_offices is not null) return _offices;
            _offices = OfficeSeedItems.ToList();
            return _offices;
        }
    }

    public static void ClearData() => _offices = null;
}
