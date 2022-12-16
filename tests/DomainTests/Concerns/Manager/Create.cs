using Cts.Domain.Concerns;
using Cts.Domain.Exceptions;
using Cts.TestData.Constants;

namespace DomainTests.Concerns.Manager;

public class Create
{
    [Test]
    public async Task WhenItemIsValid_CreatesItem()
    {
        var repoMock = new Mock<IConcernRepository>();
        repoMock.Setup(l => l.FindByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Concern?)null);
        var manager = new ConcernManager(repoMock.Object);

        var newItem = await manager.CreateAsync(TestConstants.ValidName);

        newItem.Name.Should().BeEquivalentTo(TestConstants.ValidName);
    }

    [Test]
    public async Task WhenItemIsInvalid_Throws()
    {
        var repoMock = new Mock<IConcernRepository>();
        repoMock.Setup(l => l.FindByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Concern(Guid.Empty, TestConstants.ValidName));
        var manager = new ConcernManager(repoMock.Object);

        var action = async () => await manager.CreateAsync(TestConstants.ValidName);

        (await action.Should().ThrowAsync<NameAlreadyExistsException>())
            .WithMessage($"An entity with that name already exists. Name: {TestConstants.ValidName}");
    }
}
