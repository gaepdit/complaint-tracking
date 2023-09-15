using AutoMapper;
using FluentAssertions.Equivalency;
using FluentAssertions.Extensions;
using Microsoft.AspNetCore.Identity;
using Cts.AppServices.AutoMapper;
using Cts.Domain.Identity;

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

        AssertionOptions.AssertEquivalencyUsing(opts => opts
            // Setting this option globally since our DTOs generally exclude properties, e.g., audit properties.
            // See: https://fluentassertions.com/objectgraphs/#matching-members
            .ExcludingMissingMembers()

            // DateTimeOffset comparison is often off by a few microseconds.
            .Using<DateTimeOffset>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, 10.Milliseconds()))
            .WhenTypeIs<DateTimeOffset>()

            // Exclude some concurrency properties automatically added by ASP.NET Identity.
            // See: https://stackoverflow.com/a/57406982/212978
            .Using(new IdentityUserSelectionRule())
        );
    }

    private class IdentityUserSelectionRule : IMemberSelectionRule
    {
        public IEnumerable<IMember> SelectMembers(INode currentNode, IEnumerable<IMember> selectedMembers,
            MemberSelectionContext context) =>
            selectedMembers.Where(e => !(e.DeclaringType.Name.StartsWith(nameof(IdentityUser)) &&
                e.Name is nameof(ApplicationUser.SecurityStamp) or nameof(ApplicationUser.ConcurrencyStamp)));

        public bool IncludesMembers => false;
        public override string ToString() => "Exclude SecurityStamp and ConcurrencyStamp from IdentityUser";
    }
}
