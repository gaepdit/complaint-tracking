using Cts.Domain.Entities.EntityBase;
using Cts.Domain.Entities.Concerns;
using Cts.TestData.Constants;

namespace DomainTests.Concerns.Entity;

public class ChangeName
{
    [Test]
    public void WithValidInput_ReturnsNewEntity()
    {
        var result = new Concern(Guid.NewGuid(), TextData.ValidName);
        result.ChangeName(TextData.NewValidName);
        result.Name.Should().Be(TextData.NewValidName);
    }

    [Test]
    public void WithEmptyName_Throws()
    {
        var result = new Concern(Guid.NewGuid(), TextData.ValidName);

        var action = () => result.ChangeName(string.Empty);

        action.Should().Throw<ArgumentException>()
            .WithMessage("Value cannot be null, empty, or white space.*");
    }

    [Test]
    public void WithShortName_Throws()
    {
        var result = new Concern(Guid.NewGuid(), TextData.ValidName);

        var action = () => result.ChangeName(TextData.ShortName);

        action.Should().Throw<ArgumentException>()
            .WithMessage($"The length must be at least the minimum length '{SimpleNamedEntity.MinNameLength}'.*");
    }
}
