using MyAppRoot.Domain.Offices;
using MyAppRoot.TestData.Constants;

namespace DomainTests.Offices.Manager;

public class Create
{
    [Test]
    public async Task WhenItemIsValid_CreatesItem()
    {
        var repoMock = new Mock<IOfficeRepository>();
        repoMock.Setup(l => l.FindByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Office?)null);
        var manager = new OfficeManager(repoMock.Object);

        var newItem = await manager.CreateAsync(TestConstants.ValidName);

        newItem.Name.Should().BeEquivalentTo(TestConstants.ValidName);
    }

    [Test]
    public async Task WhenItemIsInvalid_Throws()
    {
        var repoMock = new Mock<IOfficeRepository>();
        repoMock.Setup(l => l.FindByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Office(Guid.Empty, TestConstants.ValidName));
        var manager = new OfficeManager(repoMock.Object);

        var office = async () => await manager.CreateAsync(TestConstants.ValidName);

        (await office.Should().ThrowAsync<OfficeNameAlreadyExistsException>())
            .WithMessage($"An Office with that name already exists. Name: {TestConstants.ValidName}");
    }
}
