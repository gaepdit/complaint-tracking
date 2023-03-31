using Cts.Domain.Entities.BaseEntities;
using Cts.Domain.Entities.Concerns;
using Cts.TestData.Constants;

namespace DomainTests.Concerns.Entity;

public class ChangeName
{
    [Test]
    public void WithValidInput_ReturnsNewEntity()
    {
        var result = new Concern(Guid.NewGuid(), TestConstants.ValidName);
        result.ChangeName(TestConstants.NewValidName);
        result.Name.Should().Be(TestConstants.NewValidName);
    }

    [Test]
    public void WithEmptyName_Throws()
    {
        var result = new Concern(Guid.NewGuid(), TestConstants.ValidName);

        var action = () => result.ChangeName(string.Empty);

        action.Should().Throw<ArgumentException>()
            .WithMessage("Value cannot be null, empty, or white space.*");
    }

    [Test]
    public void WithShortName_Throws()
    {
        var result = new Concern(Guid.NewGuid(), TestConstants.ValidName);

        var action = () => result.ChangeName(TestConstants.ShortName);

        action.Should().Throw<ArgumentException>()
            .WithMessage($"The length must be at least the minimum length '{SimpleNamedEntity.MinNameLength}'.*");
    }
}
