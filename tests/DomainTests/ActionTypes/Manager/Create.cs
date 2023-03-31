using Cts.Domain.Entities.ActionTypes;
using Cts.Domain.Exceptions;
using Cts.TestData.Constants;

namespace DomainTests.ActionTypes.Manager;

public class Create
{
    [Test]
    public async Task WhenItemIsValid_CreatesItem()
    {
        var repoMock = new Mock<IActionTypeRepository>();
        repoMock.Setup(l => l.FindByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ActionType?)null);
        var manager = new ActionTypeManager(repoMock.Object);

        var newItem = await manager.CreateAsync(TestConstants.ValidName);

        newItem.Name.Should().BeEquivalentTo(TestConstants.ValidName);
    }

    [Test]
    public async Task WhenItemIsInvalid_Throws()
    {
        var repoMock = new Mock<IActionTypeRepository>();
        repoMock.Setup(l => l.FindByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ActionType(Guid.Empty, TestConstants.ValidName));
        var manager = new ActionTypeManager(repoMock.Object);

        var action = async () => await manager.CreateAsync(TestConstants.ValidName);

        (await action.Should().ThrowAsync<NameAlreadyExistsException>())
            .WithMessage($"An entity with that name already exists. Name: {TestConstants.ValidName}");
    }
}
