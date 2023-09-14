using Cts.Domain.Entities.EntityBase;
using Cts.Domain.Entities.Concerns;
using Cts.Domain.Exceptions;
using Cts.TestData.Constants;

namespace DomainTests.Concerns.Manager;

public class ChangeName
{
    [Test]
    public async Task WhenNewNameIsValid_ChangesName()
    {
        var item = new Concern(Guid.Empty, TextData.ValidName);
        var repoMock = Substitute.For<IConcernRepository>();
        repoMock.FindByNameAsync(TextData.NewValidName, Arg.Any<CancellationToken>())
            .Returns((Concern?)null);
        var manager = new ConcernManager(repoMock);

        await manager.ChangeNameAsync(item, TextData.NewValidName);

        item.Name.Should().BeEquivalentTo(TextData.NewValidName);
    }

    [Test]
    public async Task WhenNewNameIsUnchanged_CompletesWithNoChange()
    {
        var item = new Concern(Guid.Empty, TextData.ValidName);
        var repoMock = Substitute.For<IConcernRepository>();
        repoMock.FindByNameAsync(TextData.ValidName, Arg.Any<CancellationToken>())
            .Returns(item);
        var manager = new ConcernManager(repoMock);

        await manager.ChangeNameAsync(item, TextData.ValidName);

        item.Name.Should().BeEquivalentTo(TextData.ValidName);
    }

    [Test]
    public async Task WhenNewNameAlreadyExists_Throws()
    {
        var item = new Concern(Guid.Empty, TextData.ValidName);
        var existingItem = new Concern(Guid.NewGuid(), TextData.NewValidName);
        var repoMock = Substitute.For<IConcernRepository>();
        repoMock.FindByNameAsync(TextData.NewValidName, Arg.Any<CancellationToken>())
            .Returns(existingItem);
        var manager = new ConcernManager(repoMock);

        var action = async () => await manager.ChangeNameAsync(item, TextData.NewValidName);

        (await action.Should().ThrowAsync<NameAlreadyExistsException>())
            .WithMessage($"An entity with that name already exists. Name: {TextData.NewValidName}");
    }

    [Test]
    public async Task WhenNewNameIsInvalid_Throws()
    {
        var item = new Concern(Guid.Empty, TextData.ValidName);
        var repoMock = Substitute.For<IConcernRepository>();
        repoMock.FindByNameAsync(TextData.NewValidName, Arg.Any<CancellationToken>())
            .Returns((Concern?)null);
        var manager = new ConcernManager(repoMock);

        var action = async () => await manager.ChangeNameAsync(item, TextData.ShortName);

        (await action.Should().ThrowAsync<ArgumentException>())
            .WithMessage($"The length must be at least the minimum length '{SimpleNamedEntity.MinNameLength}'.*");
    }
}
