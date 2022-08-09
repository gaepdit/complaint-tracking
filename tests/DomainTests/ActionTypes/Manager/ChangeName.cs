using Cts.Domain.ActionTypes;
using Cts.Domain.Entities;
using Cts.TestData.ActionTypes;

namespace DomainTests.ActionTypes.Manager;

public class ChangeName
{
    [Test]
    public async Task WhenNewNameIsValid_ChangesName()
    {
        var item = new ActionType(Guid.Empty, ActionTypeConstants.ValidName);
        var repoMock = new Mock<IActionTypeRepository>();
        repoMock.Setup(l => l.FindByNameAsync(ActionTypeConstants.NewValidName, default))
            .ReturnsAsync((ActionType?)null);
        var manager = new ActionTypeManager(repoMock.Object);

        await manager.ChangeNameAsync(item, ActionTypeConstants.NewValidName);

        item.Name.Should().BeEquivalentTo(ActionTypeConstants.NewValidName);
    }

    [Test]
    public async Task WhenNewNameIsUnchanged_CompletesWithNoChange()
    {
        var item = new ActionType(Guid.Empty, ActionTypeConstants.ValidName);
        var repoMock = new Mock<IActionTypeRepository>();
        repoMock.Setup(l => l.FindByNameAsync(ActionTypeConstants.ValidName, default))
            .ReturnsAsync(item);
        var manager = new ActionTypeManager(repoMock.Object);

        await manager.ChangeNameAsync(item, ActionTypeConstants.ValidName);

        item.Name.Should().BeEquivalentTo(ActionTypeConstants.ValidName);
    }

    [Test]
    public async Task WhenNewNameAlreadyExists_Throws()
    {
        var item = new ActionType(Guid.Empty, ActionTypeConstants.ValidName);
        var existingItem = new ActionType(Guid.NewGuid(), ActionTypeConstants.NewValidName);
        var repoMock = new Mock<IActionTypeRepository>();
        repoMock.Setup(l => l.FindByNameAsync(ActionTypeConstants.NewValidName, default))
            .ReturnsAsync(existingItem);
        var manager = new ActionTypeManager(repoMock.Object);

        var action = async () => await manager.ChangeNameAsync(item, ActionTypeConstants.NewValidName);

        (await action.Should().ThrowAsync<ActionTypeNameAlreadyExistsException>())
            .WithMessage($"An Action Type with that name already exists. Name: {ActionTypeConstants.NewValidName}");
    }

    [Test]
    public async Task WhenNewNameIsInvalid_Throws()
    {
        var item = new ActionType(Guid.Empty, ActionTypeConstants.ValidName);
        var repoMock = new Mock<IActionTypeRepository>();
        repoMock.Setup(l => l.FindByNameAsync(ActionTypeConstants.NewValidName, default))
            .ReturnsAsync((ActionType?)null);
        var manager = new ActionTypeManager(repoMock.Object);

        var action = async () => await manager.ChangeNameAsync(item, ActionTypeConstants.ShortName);

        (await action.Should().ThrowAsync<ArgumentException>())
            .WithMessage($"The length must be at least the minimum length '{ActionType.MinNameLength}'.*");
    }
}
