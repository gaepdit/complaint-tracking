using Cts.Domain.Entities;
using Cts.TestData.ActionTypes;

namespace DomainTests.ActionTypes.Entity;

public class ChangeName
{
    [Test]
    public void WithValidInput_ReturnsNewEntity()
    {
        var result = new ActionType(Guid.NewGuid(), ActionTypeConstants.ValidName);
        result.ChangeName(ActionTypeConstants.NewValidName);
        result.Name.Should().Be(ActionTypeConstants.NewValidName);
    }

    [Test]
    public void WithEmptyName_Throws()
    {
        var result = new ActionType(Guid.NewGuid(), ActionTypeConstants.ValidName);

        var action = () => result.ChangeName(string.Empty);

        action.Should().Throw<ArgumentException>()
            .WithMessage("Value cannot be null, empty, or white space.*");
    }

    [Test]
    public void WithShortName_Throws()
    {
        var result = new ActionType(Guid.NewGuid(), ActionTypeConstants.ValidName);

        var action = () => result.ChangeName(ActionTypeConstants.ShortName);

        action.Should().Throw<ArgumentException>()
            .WithMessage($"The length must be at least the minimum length '{ActionType.MinNameLength}'.*");
    }
}
