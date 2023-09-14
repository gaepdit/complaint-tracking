using Cts.Domain.Entities.Concerns;
using Cts.Domain.Exceptions;
using Cts.TestData.Constants;

namespace DomainTests.Concerns.Manager;

public class Create
{
    [Test]
    public async Task WhenItemIsValid_CreatesItem()
    {
        var repoMock = Substitute.For<IConcernRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns((Concern?)null);
        var manager = new ConcernManager(repoMock);

        var newItem = await manager.CreateAsync(TextData.ValidName, null);

        newItem.Name.Should().BeEquivalentTo(TextData.ValidName);
    }

    [Test]
    public async Task WhenItemIsInvalid_Throws()
    {
        var repoMock = Substitute.For<IConcernRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new Concern(Guid.Empty, TextData.ValidName));
        var manager = new ConcernManager(repoMock);

        var action = async () => await manager.CreateAsync(TextData.ValidName, null);

        (await action.Should().ThrowAsync<NameAlreadyExistsException>())
            .WithMessage($"An entity with that name already exists. Name: {TextData.ValidName}");
    }
}
