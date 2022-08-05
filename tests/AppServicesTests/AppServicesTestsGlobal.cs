using AutoMapper;
using Cts.AppServices.ActionTypes;
using Cts.AppServices.Offices;
using Cts.AppServices.Users;

namespace AppServicesTests;

[SetUpFixture]
public class AppServicesTestsGlobal
{
    internal static IMapper? Mapper;
    internal static MapperConfiguration? MapperConfig;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        // All AutoMapper profiles are added here.
        MapperConfig = new MapperConfiguration(c =>
        {
            c.AddProfile(new ActionTypeMappingProfile());
            c.AddProfile(new OfficeMappingProfile());
            c.AddProfile(new UsersMappingProfile());
        });

        Mapper = MapperConfig.CreateMapper();

        // Setting this option globally since our DTOs generally exclude properties, e.g., audit properties.
        // See https://fluentassertions.com/objectgraphs/#global-configuration
        // and https://fluentassertions.com/objectgraphs/#matching-members
        AssertionOptions.AssertEquivalencyUsing(opts =>
            opts.ExcludingMissingMembers());
    }
}
