using AutoMapper;
using Cts.AppServices.Concerns;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities.Concerns;
using Cts.TestData.Constants;

namespace AppServicesTests.Concerns;

public class FindForUpdate
{
    [Test]
    public async Task WhenItemExists_ReturnsViewDto()
    {
        var item = new Concern(Guid.Empty, TextData.ValidName);
        var repoMock = Substitute.For<IConcernRepository>();
        repoMock.FindAsync(item.Id).Returns(item);
        var managerMock = Substitute.For<IConcernManager>();
        var userServiceMock = Substitute.For<IUserService>();
        var appService = new ConcernService(repoMock, managerMock,
            AppServicesTestsSetup.Mapper!, userServiceMock);

        var result = await appService.FindForUpdateAsync(Guid.Empty);

        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsNull()
    {
        var id = Guid.Empty;
        var repoMock = Substitute.For<IConcernRepository>();
        repoMock.FindAsync(id).Returns((Concern?)null);
        var managerMock = Substitute.For<IConcernManager>();
        var mapperMock = Substitute.For<IMapper>();
        var userServiceMock = Substitute.For<IUserService>();
        var appService = new ConcernService(repoMock, managerMock,
            mapperMock, userServiceMock);

        var result = await appService.FindForUpdateAsync(Guid.Empty);

        result.Should().BeNull();
    }
}
