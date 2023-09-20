using FluentAssertions.Equivalency;
using Microsoft.AspNetCore.Identity;
using Cts.Domain.Identity;

namespace LocalRepositoryTests;

[SetUpFixture]
public class LocalRepositoryTestsSetup
{
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        AssertionOptions.AssertEquivalencyUsing(opts => opts
            // Exclude some concurrency properties automatically added by ASP.NET Identity.
            // See: https://stackoverflow.com/a/57406982/212978
            .Using(new IdentityUserSelectionRule())
        );
    }
}

internal class IdentityUserSelectionRule : IMemberSelectionRule
{
    public IEnumerable<IMember> SelectMembers(INode currentNode, IEnumerable<IMember> selectedMembers,
        MemberSelectionContext context) =>
        selectedMembers.Where(e => !(e.DeclaringType.Name.StartsWith(nameof(IdentityUser)) &&
            e.Name is nameof(ApplicationUser.SecurityStamp) or nameof(ApplicationUser.ConcurrencyStamp)));

    public bool IncludesMembers => false;
    public override string ToString() => "Exclude SecurityStamp and ConcurrencyStamp from IdentityUser";
}
