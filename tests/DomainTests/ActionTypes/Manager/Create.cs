using Cts.Domain.Entities.ActionTypes;
using Cts.Domain.Exceptions;
using Cts.TestData.Constants;

namespace DomainTests.ActionTypes.Manager;

public class Create
{
    [Test]
    public async Task WhenItemIsValid_CreatesItem()
    {
        var repoMock = Substitute.For<IActionTypeRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns((ActionType?)null);
        var manager = new ActionTypeManager(repoMock);

        var newItem = await manager.CreateAsync(TextData.ValidName, null);

        newItem.Name.Should().BeEquivalentTo(TextData.ValidName);
    }

    [Test]
    public async Task WhenItemIsInvalid_Throws()
    {
        var repoMock = Substitute.For<IActionTypeRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new ActionType(Guid.Empty, TextData.ValidName));
        var manager = new ActionTypeManager(repoMock);

        var action = async () => await manager.CreateAsync(TextData.ValidName, null);

        (await action.Should().ThrowAsync<NameAlreadyExistsException>())
            .WithMessage($"An entity with that name already exists. Name: {TextData.ValidName}");
    }
}
