using Cts.Domain.ActionTypes;
using Cts.Domain.BaseEntities;
using Cts.Domain.Exceptions;
using Cts.TestData.Constants;

namespace DomainTests.ActionTypes.Manager;

public class ChangeName
{
    [Test]
    public async Task WhenNewNameIsValid_ChangesName()
    {
        var item = new ActionType(Guid.Empty, TestConstants.ValidName);
        var repoMock = new Mock<IActionTypeRepository>();
        repoMock.Setup(l => l.FindByNameAsync(TestConstants.NewValidName, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ActionType?)null);
        var manager = new ActionTypeManager(repoMock.Object);

        await manager.ChangeNameAsync(item, TestConstants.NewValidName);

        item.Name.Should().BeEquivalentTo(TestConstants.NewValidName);
    }

    [Test]
    public async Task WhenNewNameIsUnchanged_CompletesWithNoChange()
    {
        var item = new ActionType(Guid.Empty, TestConstants.ValidName);
        var repoMock = new Mock<IActionTypeRepository>();
        repoMock.Setup(l => l.FindByNameAsync(TestConstants.ValidName, It.IsAny<CancellationToken>()))
            .ReturnsAsync(item);
        var manager = new ActionTypeManager(repoMock.Object);

        await manager.ChangeNameAsync(item, TestConstants.ValidName);

        item.Name.Should().BeEquivalentTo(TestConstants.ValidName);
    }

    [Test]
    public async Task WhenNewNameAlreadyExists_Throws()
    {
        var item = new ActionType(Guid.Empty, TestConstants.ValidName);
        var existingItem = new ActionType(Guid.NewGuid(), TestConstants.NewValidName);
        var repoMock = new Mock<IActionTypeRepository>();
        repoMock.Setup(l => l.FindByNameAsync(TestConstants.NewValidName, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingItem);
        var manager = new ActionTypeManager(repoMock.Object);

        var action = async () => await manager.ChangeNameAsync(item, TestConstants.NewValidName);

        (await action.Should().ThrowAsync<NameAlreadyExistsException>())
            .WithMessage($"An entity with that name already exists. Name: {TestConstants.NewValidName}");
    }

    [Test]
    public async Task WhenNewNameIsInvalid_Throws()
    {
        var item = new ActionType(Guid.Empty, TestConstants.ValidName);
        var repoMock = new Mock<IActionTypeRepository>();
        repoMock.Setup(l => l.FindByNameAsync(TestConstants.NewValidName, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ActionType?)null);
        var manager = new ActionTypeManager(repoMock.Object);

        var action = async () => await manager.ChangeNameAsync(item, TestConstants.ShortName);

        (await action.Should().ThrowAsync<ArgumentException>())
            .WithMessage($"The length must be at least the minimum length '{SimpleNamedEntity.MinNameLength}'.*");
    }
}
