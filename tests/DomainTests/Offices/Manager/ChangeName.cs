using Cts.Domain.Entities.Offices;
using Cts.Domain.Exceptions;
using Cts.TestData.Constants;

namespace DomainTests.Offices.Manager;

public class ChangeName
{
    [Test]
    public async Task WhenNewNameIsValid_ChangesName()
    {
        var item = new Office(Guid.Empty, TestConstants.ValidName);
        var repoMock = Substitute.For<IOfficeRepository>();
        repoMock.FindByNameAsync(TestConstants.NewValidName, Arg.Any<CancellationToken>())
            .Returns((Office?)null);
        var manager = new OfficeManager(repoMock);

        await manager.ChangeNameAsync(item, TestConstants.NewValidName);

        item.Name.Should().BeEquivalentTo(TestConstants.NewValidName);
    }

    [Test]
    public async Task WhenNewNameIsUnchanged_CompletesWithNoChange()
    {
        var item = new Office(Guid.Empty, TestConstants.ValidName);
        var repoMock = Substitute.For<IOfficeRepository>();
        repoMock.FindByNameAsync(TestConstants.ValidName, Arg.Any<CancellationToken>())
            .Returns(item);
        var manager = new OfficeManager(repoMock);

        await manager.ChangeNameAsync(item, TestConstants.ValidName);

        item.Name.Should().BeEquivalentTo(TestConstants.ValidName);
    }

    [Test]
    public async Task WhenNewNameAlreadyExists_Throws()
    {
        var item = new Office(Guid.Empty, TestConstants.ValidName);
        var existingItem = new Office(Guid.NewGuid(), TestConstants.NewValidName);
        var repoMock = Substitute.For<IOfficeRepository>();
        repoMock.FindByNameAsync(TestConstants.NewValidName, Arg.Any<CancellationToken>())
            .Returns(existingItem);
        var manager = new OfficeManager(repoMock);

        var action = async () => await manager.ChangeNameAsync(item, TestConstants.NewValidName);

        (await action.Should().ThrowAsync<NameAlreadyExistsException>())
            .WithMessage($"An entity with that name already exists. Name: {TestConstants.NewValidName}");
    }

    [Test]
    public async Task WhenNewNameIsInvalid_Throws()
    {
        var item = new Office(Guid.Empty, TestConstants.ValidName);
        var repoMock = Substitute.For<IOfficeRepository>();
        repoMock.FindByNameAsync(TestConstants.NewValidName, Arg.Any<CancellationToken>())
            .Returns((Office?)null);
        var manager = new OfficeManager(repoMock);

        var action = async () => await manager.ChangeNameAsync(item, TestConstants.ShortName);

        (await action.Should().ThrowAsync<ArgumentException>())
            .WithMessage($"The length must be at least the minimum length '{Office.MinNameLength}'.*");
    }
}
