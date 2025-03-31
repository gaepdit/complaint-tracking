using AutoMapper;
using Cts.AppServices.ActionTypes;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities.ActionTypes;
using Cts.TestData.Constants;

namespace AppServicesTests.ActionTypes;

public class FindForUpdate
{
    [Test]
    public async Task WhenItemExists_ReturnsViewDto()
    {
        var item = new ActionType(Guid.Empty, TextData.ValidName);
        var repoMock = Substitute.For<IActionTypeRepository>();
        repoMock.FindAsync(item.Id)
            .Returns(item);
        var managerMock = Substitute.For<IActionTypeManager>();
        var userServiceMock = Substitute.For<IUserService>();
        var appService = new ActionTypeService(repoMock, managerMock,
            AppServicesTestsSetup.Mapper!, userServiceMock);

        var result = await appService.FindForUpdateAsync(Guid.Empty);

        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsNull()
    {
        var id = Guid.Empty;
        var repoMock = Substitute.For<IActionTypeRepository>();
        repoMock.FindAsync(id)
            .Returns((ActionType?)null);
        var managerMock = Substitute.For<IActionTypeManager>();
        var mapperMock = Substitute.For<IMapper>();
        var userServiceMock = Substitute.For<IUserService>();
        var appService = new ActionTypeService(repoMock, managerMock,
            mapperMock, userServiceMock);

        var result = await appService.FindForUpdateAsync(Guid.Empty);

        result.Should().BeNull();
    }
}
