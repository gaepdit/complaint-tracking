using Cts.AppServices.Concerns;
using Cts.AppServices.UserServices;
using Cts.Domain.Concerns;
using Cts.Domain.Identity;
using Cts.TestData.Constants;

namespace AppServicesTests.Concerns;

public class Create
{
    [Test]
    public async Task WhenResourceIsValid_ReturnsId()
    {
        var item = new Concern(Guid.NewGuid(), TestConstants.ValidName);
        var repoMock = new Mock<IConcernRepository>();
        var managerMock = new Mock<IConcernManager>();
        managerMock.Setup(l => l.CreateAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(item);
        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(l => l.GetCurrentUserAsync())
            .ReturnsAsync((ApplicationUser?)null);
        var appService = new ConcernAppService(repoMock.Object, managerMock.Object,
            AppServicesTestsGlobal.Mapper!, userServiceMock.Object);

        var result = await appService.CreateAsync(item.Name);

        result.Should().Be(item.Id);
    }
}
