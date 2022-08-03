using Cts.Domain.ActionTypes;
using Cts.TestData.ActionTypes;

namespace DomainTests.ActionTypes.Manager;

public class Create
{
    [Test]
    public async Task WhenItemIsValid_CreatesItem()
    {
        var repo = new Mock<IActionTypeRepository>();
        repo.Setup(l => l.FindByNameAsync(It.IsAny<string>(), default))
            .ReturnsAsync((ActionType?)null);
        var manager = new ActionTypeManager(repo.Object);

        var newItem = await manager.CreateAsync(ActionTypeConstants.ValidName);

        newItem.Name.Should().BeEquivalentTo(ActionTypeConstants.ValidName);
    }

    [Test]
    public async Task WhenItemIsInvalid_Throws()
    {
        var repo = new Mock<IActionTypeRepository>();
        repo.Setup(l => l.FindByNameAsync(It.IsAny<string>(), default))
            .ReturnsAsync(new ActionType(Guid.Empty, ActionTypeConstants.ValidName));
        var manager = new ActionTypeManager(repo.Object);

        var action = async () => await manager.CreateAsync(ActionTypeConstants.ValidName);

        (await action.Should().ThrowAsync<ActionTypeNameAlreadyExistsException>())
            .WithMessage($"An Action Type with that name already exists. Name: {ActionTypeConstants.ValidName}");
    }
}
