using Cts.Domain.Offices;
using Cts.Domain.Entities;
using Cts.TestData.Offices;

namespace DomainTests.Offices.Manager;

public class ChangeName
{
    [Test]
    public async Task WhenNewNameIsValid_ChangesName()
    {
        var item = new Office(Guid.Empty, OfficeConstants.ValidName);
        var repoMock = new Mock<IOfficeRepository>();
        repoMock.Setup(l => l.FindByNameAsync(OfficeConstants.NewValidName, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Office?)null);
        var manager = new OfficeManager(repoMock.Object);

        await manager.ChangeNameAsync(item, OfficeConstants.NewValidName);

        item.Name.Should().BeEquivalentTo(OfficeConstants.NewValidName);
    }

    [Test]
    public async Task WhenNewNameIsUnchanged_CompletesWithNoChange()
    {
        var item = new Office(Guid.Empty, OfficeConstants.ValidName);
        var repoMock = new Mock<IOfficeRepository>();
        repoMock.Setup(l => l.FindByNameAsync(OfficeConstants.ValidName, It.IsAny<CancellationToken>()))
            .ReturnsAsync(item);
        var manager = new OfficeManager(repoMock.Object);

        await manager.ChangeNameAsync(item, OfficeConstants.ValidName);

        item.Name.Should().BeEquivalentTo(OfficeConstants.ValidName);
    }

    [Test]
    public async Task WhenNewNameAlreadyExists_Throws()
    {
        var item = new Office(Guid.Empty, OfficeConstants.ValidName);
        var existingItem = new Office(Guid.NewGuid(), OfficeConstants.NewValidName);
        var repoMock = new Mock<IOfficeRepository>();
        repoMock.Setup(l => l.FindByNameAsync(OfficeConstants.NewValidName, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingItem);
        var manager = new OfficeManager(repoMock.Object);

        var action = async () => await manager.ChangeNameAsync(item, OfficeConstants.NewValidName);

        (await action.Should().ThrowAsync<OfficeNameAlreadyExistsException>())
            .WithMessage($"An Office with that name already exists. Name: {OfficeConstants.NewValidName}");
    }

    [Test]
    public async Task WhenNewNameIsInvalid_Throws()
    {
        var item = new Office(Guid.Empty, OfficeConstants.ValidName);
        var repoMock = new Mock<IOfficeRepository>();
        repoMock.Setup(l => l.FindByNameAsync(OfficeConstants.NewValidName, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Office?)null);
        var manager = new OfficeManager(repoMock.Object);

        var action = async () => await manager.ChangeNameAsync(item, OfficeConstants.ShortName);

        (await action.Should().ThrowAsync<ArgumentException>())
            .WithMessage($"The length must be at least the minimum length '{Office.MinNameLength}'.*");
    }
}
