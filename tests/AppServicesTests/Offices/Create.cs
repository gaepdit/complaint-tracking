using Cts.AppServices.Offices;
using Cts.AppServices.UserServices;
using Cts.Domain.Identity;
using Cts.Domain.Offices;
using Cts.TestData.Constants;

namespace AppServicesTests.Offices;

public class Create
{
    [Test]
    public async Task WhenResourceIsValid_ReturnsId()
    {
        var item = new Office(Guid.NewGuid(), TestConstants.ValidName);
        var repoMock = new Mock<IOfficeRepository>();
        var managerMock = new Mock<IOfficeManager>();
        managerMock.Setup(l =>
                l.CreateAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(item);
        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(l => l.GetCurrentUserAsync())
            .ReturnsAsync((ApplicationUser?)null);
        var appService = new OfficeAppService(repoMock.Object, managerMock.Object,
            AppServicesTestsGlobal.Mapper!, userServiceMock.Object);
        var resource = new OfficeCreateDto { Name = TestConstants.ValidName };

        var result = await appService.CreateAsync(resource);

        result.Should().Be(item.Id);
    }
}
