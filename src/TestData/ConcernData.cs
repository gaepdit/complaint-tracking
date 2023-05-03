using Cts.Domain.Entities.Concerns;

namespace Cts.TestData;

internal static class ConcernData
{
    private static IEnumerable<Concern> ConcernSeedItems => new List<Concern>
    {
        new(new Guid("50000000-0000-0000-0000-000000000030"), "Agricultural Ground Water Use"),
        new(new Guid("50000000-0000-0000-0000-000000000031"), "Agricultural Surface Water Use"),
        new(new Guid("50000000-0000-0000-0000-000000000032"), "Air Quality Control"),
        new(new Guid("50000000-0000-0000-0000-000000000033"), "Asbestos"),
        new(new Guid("50000000-0000-0000-0000-000000000034"), "Clean Fueled Fleets"),
        new(new Guid("50000000-0000-0000-0000-000000000035"), "Comprehensive Solid Waste"),
        new(new Guid("50000000-0000-0000-0000-000000000036"), "Diesel fuel spill"),
        new(new Guid("50000000-0000-0000-0000-000000000037"), "Enhanced Inspection & Maintenance"),
        new(new Guid("50000000-0000-0000-0000-000000000038"), "Environmental Policy"),
        new(new Guid("50000000-0000-0000-0000-000000000039"), "Environmentally Sensitive Properties"),
        new(new Guid("50000000-0000-0000-0000-000000000040"), "Erosion & Sedimentation Control"),
        new(new Guid("50000000-0000-0000-0000-000000000041"), "Gasoline fuel spill"),
        new(new Guid("50000000-0000-0000-0000-000000000042"), "Grants"),
        new(new Guid("50000000-0000-0000-0000-000000000043"), "Ground Water & Surface Water Withdrawals"),
        new(new Guid("50000000-0000-0000-0000-000000000044"), "Ground Water Use"),
        new(new Guid("50000000-0000-0000-0000-000000000045"), "Hazardous Site Response"),
        new(new Guid("50000000-0000-0000-0000-000000000046"), "Hazardous Waste Management"),
        new(new Guid("50000000-0000-0000-0000-000000000047"), "Hazardous material spill"),
        new(new Guid("50000000-0000-0000-0000-000000000048"), "Lead paint abatement"),
        new(new Guid("50000000-0000-0000-0000-000000000049"), "Motor Vehicle Inspection & Maintenance"),
        new(new Guid("50000000-0000-0000-0000-000000000050"), "Nuclear Power incident"),
        new(new Guid("50000000-0000-0000-0000-000000000051"), "Oil & Gas Deep Drilling"),
        new(new Guid("50000000-0000-0000-0000-000000000052"), "Oil spill"),
        new(new Guid("50000000-0000-0000-0000-000000000053"), "Radioactive Materials - Allegation"),
        new(new Guid("50000000-0000-0000-0000-000000000054"), "Radioactive Materials - DOT Exemption"),
        new(new Guid("50000000-0000-0000-0000-000000000055"), "Radioactive Materials - Incident"),
        new(new Guid("50000000-0000-0000-0000-000000000056"), "Radioactive Waste Disposal"),
        new(new Guid("50000000-0000-0000-0000-000000000057"), "Safe Dams"),
        new(new Guid("50000000-0000-0000-0000-000000000058"), "Safe Drinking Water"),
        new(new Guid("50000000-0000-0000-0000-000000000059"), "Scrap Tire"),
        new(new Guid("50000000-0000-0000-0000-000000000060"), "Sewage spill"),
        new(new Guid("50000000-0000-0000-0000-000000000061"), "Sewerage Holding Tanks"),
        new(new Guid("50000000-0000-0000-0000-000000000062"), "Surface Mining"),
        new(new Guid("50000000-0000-0000-0000-000000000063"), "Underground Gas Storage"),
        new(new Guid("50000000-0000-0000-0000-000000000064"), "Underground Storage Tanks"),
        new(new Guid("50000000-0000-0000-0000-000000000065"), "Water Quality Confined Animal Operations"),
        new(new Guid("50000000-0000-0000-0000-000000000066"), "Water Quality Control"),
        new(new Guid("50000000-0000-0000-0000-000000000067"), "Water Quality Stormwater Construction"),
        new(new Guid("50000000-0000-0000-0000-000000000068"), "Water Quality Stormwater Industrial"),
        new(new Guid("50000000-0000-0000-0000-000000000069"), "Water Quality Stormwater Urban"),
    };

    private static IEnumerable<Concern>? _concerns;

    public static IEnumerable<Concern> GetConcerns
    {
        get
        {
            if (_concerns is not null) return _concerns;
            _concerns = ConcernSeedItems;
            return _concerns;
        }
    }

    public static void ClearData() => _concerns = null;
}
