using AutoMapper;
using Cts.AppServices.ActionTypes;
using Cts.AppServices.Users;
using Cts.Domain.ActionTypes;
using Cts.Domain.Entities;
using Cts.TestData.ActionTypes;

namespace AppServicesTests.ActionTypes;

public class Get
{
    [Test]
    public async Task WhenItemExists_ReturnsViewDto()
    {
        var item = new ActionType(Guid.Empty, ActionTypeConstants.ValidName);
        var repoMock = new Mock<IActionTypeRepository>();
        repoMock.Setup(l => l.FindAsync(item.Id, default))
            .ReturnsAsync(item);
        var managerMock = new Mock<IActionTypeManager>();
        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(l => l.GetCurrentUserAsync())
            .ReturnsAsync((UserViewDto?)null);
        var appService = new ActionTypeAppService(repoMock.Object, managerMock.Object,
            AppServicesTestsGlobal.Mapper!, userServiceMock.Object);

        var result = await appService.FindForUpdateAsync(Guid.Empty);

        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsNull()
    {
        var id = Guid.Empty;
        var repoMock = new Mock<IActionTypeRepository>();
        repoMock.Setup(l => l.FindAsync(id, default))
            .ReturnsAsync((ActionType?)null);
        var managerMock = new Mock<IActionTypeManager>();
        var mapperMock = new Mock<IMapper>();
        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(l => l.GetCurrentUserAsync())
            .ReturnsAsync((UserViewDto?)null);
        var appService = new ActionTypeAppService(repoMock.Object, managerMock.Object,
            mapperMock.Object, userServiceMock.Object);

        var result = await appService.FindForUpdateAsync(Guid.Empty);

        result.Should().BeNull();
    }
}
