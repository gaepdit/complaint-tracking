using Cts.Domain.ActionTypes;

namespace DomainTests.ActionTypes;

public class Constructor
{
    [Test]
    public void WithValidInput_ReturnsNewEntity()
    {
        var newGuid = Guid.NewGuid();
        var result = new ActionType(newGuid, newGuid.ToString());
        Assert.Multiple(() =>
        {
            result.Id.Should().Be(newGuid);
            result.Name.Should().Be(newGuid.ToString());
            result.Active.Should().BeTrue();
        });
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
        var action = () => new ActionType(Guid.Empty, "a");
        action.Should().Throw<ArgumentException>()
            .WithMessage($"The length must be at least the minimum length '{ActionType.MinNameLength}'.*");
    }
}
