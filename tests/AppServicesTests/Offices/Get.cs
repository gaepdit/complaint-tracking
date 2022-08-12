using AutoMapper;
using Cts.AppServices.Offices;
using Cts.AppServices.Users;
using Cts.Domain.Offices;
using Cts.Domain.Entities;
using Cts.TestData.Offices;

namespace AppServicesTests.Offices;

public class Get
{
    [Test]
    public async Task WhenItemExists_ReturnsViewDto()
    {
        var item = new Office(Guid.Empty, OfficeConstants.ValidName);
        var repoMock = new Mock<IOfficeRepository>();
        repoMock.Setup(l => l.FindAsync(item.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(item);
        var managerMock = new Mock<IOfficeManager>();
        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(l => l.GetCurrentUserAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync((UserViewDto?)null);
        var appService = new OfficeAppService(repoMock.Object, managerMock.Object,
            AppServicesTestsGlobal.Mapper!, userServiceMock.Object);

        var result = await appService.FindForUpdateAsync(Guid.Empty);

        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsNull()
    {
        var id = Guid.Empty;
        var repoMock = new Mock<IOfficeRepository>();
        repoMock.Setup(l => l.FindAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Office?)null);
        var managerMock = new Mock<IOfficeManager>();
        var mapperMock = new Mock<IMapper>();
        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(l => l.GetCurrentUserAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync((UserViewDto?)null);
        var appService = new OfficeAppService(repoMock.Object, managerMock.Object,
            mapperMock.Object, userServiceMock.Object);

        var result = await appService.FindForUpdateAsync(Guid.Empty);

        result.Should().BeNull();
    }
}
