using Cts.Domain.Entities;

namespace Cts.TestData.Offices;

internal static class OfficeData
{
    private static List<Office> OfficeSeedItems(ApplicationUser user) =>
        new()
        {
            new Office(Guid.NewGuid(), "Air Protection Branch", user) { Active = true },
            new Office(Guid.NewGuid(), "Coastal District", user) { Active = true },
            new Office(Guid.NewGuid(), "Director's Office", user) { Active = true },
            new Office(Guid.NewGuid(), "East Central District/Augusta", user) { Active = true },
            new Office(Guid.NewGuid(), "Emergency Response Team", user) { Active = true },
            new Office(Guid.NewGuid(), "Geologic Survey Branch", user) { Active = true },
            new Office(Guid.NewGuid(), "Land Protection Branch", user) { Active = true },
            new Office(Guid.NewGuid(), "Middle Region", user) { Active = true },
            new Office(Guid.NewGuid(), "Mountain District", user) { Active = true },
            new Office(Guid.NewGuid(), "Northeast Region", user) { Active = true },
            new Office(Guid.NewGuid(), "Small Business Environmental Assistance Program", user) { Active = true },
            new Office(Guid.NewGuid(), "Southwest Region", user) { Active = true },
            new Office(Guid.NewGuid(), "Water Resources Branch", user) { Active = true },
            new Office(Guid.NewGuid(), "Watershed Protection Branch", user) { Active = true },
            new Office(Guid.NewGuid(), "Air Laboratory", user) { Active = false },
            // null user in "Air Protection Branch (Not used)"
            new Office(Guid.NewGuid(), "Air Protection Branch (Not used)") { Active = false },
            new Office(Guid.NewGuid(), "Bacteriology Laboratory", user) { Active = false },
            new Office(Guid.NewGuid(), "Environmental Toxicology", user) { Active = false },
            new Office(Guid.NewGuid(), "Information Management", user) { Active = false },
            new Office(Guid.NewGuid(), "Inorganic Laboratory", user) { Active = false },
            new Office(Guid.NewGuid(), "Laboratories", user) { Active = false },
            new Office(Guid.NewGuid(), "Metals Laboratory", user) { Active = false },
            new Office(Guid.NewGuid(), "Northwest Region", user) { Active = false },
            new Office(Guid.NewGuid(), "Organic Laboratory", user) { Active = false },
            new Office(Guid.NewGuid(), "Radioactive Material Program (not used)", user) { Active = false },
            new Office(Guid.NewGuid(), "Radioactive Surveillance Program (not used)", user) { Active = false },
            new Office(Guid.NewGuid(), "Right To Know/SARA Title III", user) { Active = false },
            new Office(Guid.NewGuid(), "Training", user) { Active = false },
        };

    private static ICollection<Office>? _offices;

    public static IEnumerable<Office> GetOffices
    {
        get
        {
            if (_offices is not null) return _offices;

            // Seed offices and user data.
            _offices = OfficeSeedItems(Identity.Data.GetUsers.First());
            _offices.First(e => e.Active).Users = Identity.Data.GetUsers.Where(e => e.Active).ToList();
            Identity.Data.GetUsers.First().Office = _offices.First();

            return _offices;
        }
    }
}
