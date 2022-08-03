using Cts.Domain.ActionTypes;
using Cts.TestData.ActionTypes;

namespace DomainTests.ActionTypes.Entity;

public class Constructor
{
    [Test]
    public void WithValidInput_ReturnsNewEntity()
    {
        var newGuid = Guid.NewGuid();
        var result = new ActionType(newGuid, ActionTypeConstants.ValidName);
        Assert.Multiple(() =>
        {
            result.Id.Should().Be(newGuid);
            result.Name.Should().Be(ActionTypeConstants.ValidName);
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
        var action = () => new ActionType(Guid.Empty, ActionTypeConstants.ShortName);
        action.Should().Throw<ArgumentException>()
            .WithMessage($"The length must be at least the minimum length '{ActionType.MinNameLength}'.*");
    }
}
