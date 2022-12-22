using AutoMapper;
using Cts.AppServices.Concerns;
using Cts.AppServices.UserServices;
using Cts.Domain.Concerns;
using Cts.TestData.Constants;

namespace AppServicesTests.Concerns;

public class FindForUpdate
{
    [Test]
    public async Task WhenItemExists_ReturnsViewDto()
    {
        var item = new Concern(Guid.Empty, TestConstants.ValidName);
        var repoMock = new Mock<IConcernRepository>();
        repoMock.Setup(l => l.FindAsync(item.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(item);
        var managerMock = new Mock<IConcernManager>();
        var userServiceMock = new Mock<IUserService>();
        var appService = new ConcernAppService(repoMock.Object, managerMock.Object,
            AppServicesTestsGlobal.Mapper!, userServiceMock.Object);

        var result = await appService.FindForUpdateAsync(Guid.Empty);

        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsNull()
    {
        var id = Guid.Empty;
        var repoMock = new Mock<IConcernRepository>();
        repoMock.Setup(l => l.FindAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Concern?)null);
        var managerMock = new Mock<IConcernManager>();
        var mapperMock = new Mock<IMapper>();
        var userServiceMock = new Mock<IUserService>();
        var appService = new ConcernAppService(repoMock.Object, managerMock.Object,
            mapperMock.Object, userServiceMock.Object);

        var result = await appService.FindForUpdateAsync(Guid.Empty);

        result.Should().BeNull();
    }
}
