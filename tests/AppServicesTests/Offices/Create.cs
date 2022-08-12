using Cts.AppServices.Offices;
using Cts.AppServices.Users;
using Cts.Domain.Offices;
using Cts.Domain.Entities;
using Cts.Domain.Users;
using Cts.TestData.Offices;

namespace AppServicesTests.Offices;

public class Create
{
    [Test]
    public async Task WhenResourceIsValid_ReturnsId()
    {
        var item = new Office(Guid.NewGuid(), OfficeConstants.ValidName);
        var repoMock = new Mock<IOfficeRepository>();
        var managerMock = new Mock<IOfficeManager>();
        managerMock.Setup(l =>
                l.CreateAsync(It.IsAny<string>(), It.IsAny<ApplicationUser?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(item);
        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(l => l.GetCurrentUserAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync((UserViewDto?)null);
        var appService = new OfficeAppService(repoMock.Object, managerMock.Object,
            AppServicesTestsGlobal.Mapper!, userServiceMock.Object);
        var resource = new OfficeCreateDto{Name = OfficeConstants.ValidName};

        var result = await appService.CreateAsync(resource);

        result.Should().Be(item.Id);
    }
}
