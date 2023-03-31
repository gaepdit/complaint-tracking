using Cts.Domain.Entities.BaseEntities;
using Cts.Domain.Entities.Concerns;
using Cts.TestData.Constants;
using FluentAssertions.Execution;

namespace DomainTests.Concerns.Entity;

public class Constructor
{
    [Test]
    public void WithValidInput_ReturnsNewEntity()
    {
        var newGuid = Guid.NewGuid();
        var result = new Concern(newGuid, TestConstants.ValidName);
        using (new AssertionScope())
        {
            result.Id.Should().Be(newGuid);
            result.Name.Should().Be(TestConstants.ValidName);
            result.Active.Should().BeTrue();
        }
    }

    [Test]
    public void WithEmptyName_Throws()
    {
        var action = () => new Concern(Guid.Empty, string.Empty);
        action.Should().Throw<ArgumentException>()
            .WithMessage("Value cannot be null, empty, or white space.*");
    }

    [Test]
    public void WithShortName_Throws()
    {
        var action = () => new Concern(Guid.Empty, TestConstants.ShortName);
        action.Should().Throw<ArgumentException>()
            .WithMessage($"The length must be at least the minimum length '{SimpleNamedEntity.MinNameLength}'.*");
    }
}
