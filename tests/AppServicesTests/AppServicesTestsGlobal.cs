using AutoMapper;
using MyAppRoot.AppServices.AutoMapper;

namespace AppServicesTests;

[SetUpFixture]
public class AppServicesTestsGlobal
{
    internal static IMapper? Mapper;
    internal static MapperConfiguration? MapperConfig;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        // AutoMapper profiles are added here.
        MapperConfig = new MapperConfiguration(c => c.AddProfile(new AutoMapperProfile()));
        Mapper = MapperConfig.CreateMapper();

        // Setting this option globally since our DTOs generally exclude properties, e.g., audit properties.
        // See https://fluentassertions.com/objectgraphs/#global-configuration
        // and https://fluentassertions.com/objectgraphs/#matching-members
        AssertionOptions.AssertEquivalencyUsing(opts =>
            opts.ExcludingMissingMembers());
    }
}
