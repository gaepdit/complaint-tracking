using MyAppRoot.Domain.Offices;
using MyAppRoot.TestData.Constants;

namespace DomainTests.Offices.Manager;

public class ChangeName
{
    [Test]
    public async Task WhenNewNameIsValid_ChangesName()
    {
        var item = new Office(Guid.Empty, TestConstants.ValidName);
        var repoMock = new Mock<IOfficeRepository>();
        repoMock.Setup(l => l.FindByNameAsync(TestConstants.NewValidName, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Office?)null);
        var manager = new OfficeManager(repoMock.Object);

        await manager.ChangeNameAsync(item, TestConstants.NewValidName);

        item.Name.Should().BeEquivalentTo(TestConstants.NewValidName);
    }

    [Test]
    public async Task WhenNewNameIsUnchanged_CompletesWithNoChange()
    {
        var item = new Office(Guid.Empty, TestConstants.ValidName);
        var repoMock = new Mock<IOfficeRepository>();
        repoMock.Setup(l => l.FindByNameAsync(TestConstants.ValidName, It.IsAny<CancellationToken>()))
            .ReturnsAsync(item);
        var manager = new OfficeManager(repoMock.Object);

        await manager.ChangeNameAsync(item, TestConstants.ValidName);

        item.Name.Should().BeEquivalentTo(TestConstants.ValidName);
    }

    [Test]
    public async Task WhenNewNameAlreadyExists_Throws()
    {
        var item = new Office(Guid.Empty, TestConstants.ValidName);
        var existingItem = new Office(Guid.NewGuid(), TestConstants.NewValidName);
        var repoMock = new Mock<IOfficeRepository>();
        repoMock.Setup(l => l.FindByNameAsync(TestConstants.NewValidName, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingItem);
        var manager = new OfficeManager(repoMock.Object);

        var action = async () => await manager.ChangeNameAsync(item, TestConstants.NewValidName);

        (await action.Should().ThrowAsync<OfficeNameAlreadyExistsException>())
            .WithMessage($"An Office with that name already exists. Name: {TestConstants.NewValidName}");
    }

    [Test]
    public async Task WhenNewNameIsInvalid_Throws()
    {
        var item = new Office(Guid.Empty, TestConstants.ValidName);
        var repoMock = new Mock<IOfficeRepository>();
        repoMock.Setup(l => l.FindByNameAsync(TestConstants.NewValidName, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Office?)null);
        var manager = new OfficeManager(repoMock.Object);

        var action = async () => await manager.ChangeNameAsync(item, TestConstants.ShortName);

        (await action.Should().ThrowAsync<ArgumentException>())
            .WithMessage($"The length must be at least the minimum length '{Office.MinNameLength}'.*");
    }
}
