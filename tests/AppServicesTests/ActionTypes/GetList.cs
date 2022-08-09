using Cts.AppServices.ActionTypes;
using Cts.AppServices.Users;
using Cts.Domain.ActionTypes;
using Cts.Domain.Entities;
using Cts.TestData.ActionTypes;

namespace AppServicesTests.ActionTypes;

public class GetList
{
    [Test]
    public async Task WhenItemExists_ReturnsViewDtoList()
    {
        var itemList = new List<ActionType> { new(Guid.Empty, ActionTypeConstants.ValidName) };
        var repoMock = new Mock<IActionTypeRepository>();
        repoMock.Setup(l => l.GetListAsync(default))
            .ReturnsAsync(itemList);
        var managerMock = new Mock<IActionTypeManager>();
        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(l => l.GetCurrentUserAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync((UserViewDto?)null);
        var appService = new ActionTypeAppService(repoMock.Object, managerMock.Object,
            AppServicesTestsGlobal.Mapper!, userServiceMock.Object);

        var result = await appService.GetListAsync();

        result.Should().BeEquivalentTo(itemList);
    }
}
