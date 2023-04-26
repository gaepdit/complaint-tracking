using AutoMapper;
using Cts.AppServices.AutoMapper;
using FluentAssertions.Extensions;

namespace AppServicesTests;

[SetUpFixture]
public class AppServicesTestsSetup
{
    internal static IMapper? Mapper;
    internal static MapperConfiguration? MapperConfig;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        // AutoMapper profiles are added here.
        MapperConfig = new MapperConfiguration(c => c.AddProfile(new AutoMapperProfile()));
        Mapper = MapperConfig.CreateMapper();

        AssertionOptions.AssertEquivalencyUsing(opts =>
            // Setting this option globally since our DTOs generally exclude properties, e.g., audit properties.
            // and https://fluentassertions.com/objectgraphs/#matching-members
            opts.ExcludingMissingMembers()
                // DateTimeOffset comparison is often off by a few microseconds.
                .Using<DateTimeOffset>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, 1.Milliseconds()))
                .WhenTypeIs<DateTimeOffset>()
        );
    }
}
