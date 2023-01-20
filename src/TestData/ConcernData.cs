using Cts.Domain.Concerns;

namespace Cts.TestData;

internal static class ConcernData
{
    private static List<Concern> ConcernSeedItems => new()
    {
        new Concern(new Guid("00000000-0000-0000-0000-000000000030"), "Agricultural Ground Water Use"),
        new Concern(new Guid("00000000-0000-0000-0000-000000000031"), "Agricultural Surface Water Use"),
        new Concern(new Guid("00000000-0000-0000-0000-000000000032"), "Air Quality Control"),
        new Concern(new Guid("00000000-0000-0000-0000-000000000033"), "Asbestos"),
        new Concern(new Guid("00000000-0000-0000-0000-000000000034"), "Clean Fueled Fleets"),
        new Concern(new Guid("00000000-0000-0000-0000-000000000035"), "Comprehensive Solid Waste"),
        new Concern(new Guid("00000000-0000-0000-0000-000000000036"), "Diesel fuel spill"),
        new Concern(new Guid("00000000-0000-0000-0000-000000000037"), "Enhanced Inspection & Maintenance"),
        new Concern(new Guid("00000000-0000-0000-0000-000000000038"), "Environmental Policy"),
        new Concern(new Guid("00000000-0000-0000-0000-000000000039"), "Environmentally Sensitive Properties"),
        new Concern(new Guid("00000000-0000-0000-0000-000000000040"), "Erosion & Sedimentation Control"),
        new Concern(new Guid("00000000-0000-0000-0000-000000000041"), "Gasoline fuel spill"),
        new Concern(new Guid("00000000-0000-0000-0000-000000000042"), "Grants"),
        new Concern(new Guid("00000000-0000-0000-0000-000000000043"), "Ground Water & Surface Water Withdrawals"),
        new Concern(new Guid("00000000-0000-0000-0000-000000000044"), "Ground Water Use"),
        new Concern(new Guid("00000000-0000-0000-0000-000000000045"), "Hazardous Site Response"),
        new Concern(new Guid("00000000-0000-0000-0000-000000000046"), "Hazardous Waste Management"),
        new Concern(new Guid("00000000-0000-0000-0000-000000000047"), "Hazardous material spill"),
        new Concern(new Guid("00000000-0000-0000-0000-000000000048"), "Lead paint abatement"),
        new Concern(new Guid("00000000-0000-0000-0000-000000000049"), "Motor Vehicle Inspection & Maintenance"),
        new Concern(new Guid("00000000-0000-0000-0000-000000000050"), "Nuclear Power incident"),
        new Concern(new Guid("00000000-0000-0000-0000-000000000051"), "Oil & Gas Deep Drilling"),
        new Concern(new Guid("00000000-0000-0000-0000-000000000052"), "Oil spill"),
        new Concern(new Guid("00000000-0000-0000-0000-000000000053"), "Radioactive Materials - Allegation"),
        new Concern(new Guid("00000000-0000-0000-0000-000000000054"), "Radioactive Materials - DOT Exemption"),
        new Concern(new Guid("00000000-0000-0000-0000-000000000055"), "Radioactive Materials - Incident"),
        new Concern(new Guid("00000000-0000-0000-0000-000000000056"), "Radioactive Waste Disposal"),
        new Concern(new Guid("00000000-0000-0000-0000-000000000057"), "Safe Dams"),
        new Concern(new Guid("00000000-0000-0000-0000-000000000058"), "Safe Drinking Water"),
        new Concern(new Guid("00000000-0000-0000-0000-000000000059"), "Scrap Tire"),
        new Concern(new Guid("00000000-0000-0000-0000-000000000060"), "Sewage spill"),
        new Concern(new Guid("00000000-0000-0000-0000-000000000061"), "Sewerage Holding Tanks"),
        new Concern(new Guid("00000000-0000-0000-0000-000000000062"), "Surface Mining"),
        new Concern(new Guid("00000000-0000-0000-0000-000000000063"), "Underground Gas Storage"),
        new Concern(new Guid("00000000-0000-0000-0000-000000000064"), "Underground Storage Tanks"),
        new Concern(new Guid("00000000-0000-0000-0000-000000000065"), "Water Quality Confined Animal Operations"),
        new Concern(new Guid("00000000-0000-0000-0000-000000000066"), "Water Quality Control"),
        new Concern(new Guid("00000000-0000-0000-0000-000000000067"), "Water Quality Stormwater Construction"),
        new Concern(new Guid("00000000-0000-0000-0000-000000000068"), "Water Quality Stormwater Industrial"),
        new Concern(new Guid("00000000-0000-0000-0000-000000000069"), "Water Quality Stormwater Urban"),
    };

    private static ICollection<Concern>? _concerns;

    public static ICollection<Concern> GetConcerns
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
