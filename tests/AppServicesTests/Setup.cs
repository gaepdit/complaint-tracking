using AutoMapper;
using AwesomeAssertions.Extensions;
using Cts.AppServices.AutoMapper;

namespace AppServicesTests;

[SetUpFixture]
public class Setup
{
    internal static IMapper? Mapper;
    internal static MapperConfiguration? MapperConfiguration;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        // Add AutoMapper profiles
        MapperConfiguration =
            new MapperConfiguration(configuration => configuration.AddProfile(new AutoMapperProfile()));
        Mapper = MapperConfiguration.CreateMapper();

        AssertionConfiguration.Current.Equivalency.Modify(options => options
            // Setting this option globally since our DTOs generally exclude properties, e.g., audit properties.
            // See: https://fluentassertions.com/objectgraphs/#matching-members
            .ExcludingMissingMembers()

            // DateTimeOffset comparison is often off by a few microseconds.
            .Using<DateTimeOffset>(context =>
                context.Subject.Should().BeCloseTo(context.Expectation, 10.Milliseconds()))
            .WhenTypeIs<DateTimeOffset>()
        );
    }
}
