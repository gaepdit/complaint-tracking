using Cts.Domain.Entities.ActionTypes;
using Cts.Domain.Entities.EntityBase;
using Cts.TestData.Constants;
using FluentAssertions.Execution;

namespace DomainTests.ActionTypes.Entity;

public class Constructor
{
    [Test]
    public void WithValidInput_ReturnsNewEntity()
    {
        var newGuid = Guid.NewGuid();
        var result = new ActionType(newGuid, TextData.ValidName);
        using (new AssertionScope())
        {
            result.Id.Should().Be(newGuid);
            result.Name.Should().Be(TextData.ValidName);
            result.Active.Should().BeTrue();
        }
    }

    [Test]
    public void WithEmptyName_Throws()
    {
        var action = () => new ActionType(Guid.Empty, string.Empty);
        action.Should().Throw<ArgumentException>()
            .WithMessage("Value cannot be null, empty, or white space.*");
    }

    [Test]
    public void WithShortName_Throws()
    {
        var action = () => new ActionType(Guid.Empty, TextData.ShortName);
        action.Should().Throw<ArgumentException>()
            .WithMessage($"The length must be at least the minimum length '{SimpleNamedEntity.MinNameLength}'.*");
    }
}
