using Cts.Domain.Entities;

namespace Cts.TestData.Offices;

internal static class OfficeData
{
    private static readonly List<Office> OfficeSeedItems = new()
    {
        new Office(new Guid("00000000-0000-0000-0000-000000000004"), "Air Protection Branch"),
        new Office(new Guid("00000000-0000-0000-0000-000000000005"), "Coastal District"),
        new Office(new Guid("00000000-0000-0000-0000-000000000006"), "Director's Office"),
        new Office(new Guid("00000000-0000-0000-0000-000000000007"), "Air Laboratory (not used)") { Active = false },
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
