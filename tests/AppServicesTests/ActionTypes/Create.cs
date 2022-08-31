using Cts.AppServices.ActionTypes;
using Cts.AppServices.UserServices;
using Cts.Domain.ActionTypes;
using Cts.Domain.Entities;
using Cts.TestData.Constants;

namespace AppServicesTests.ActionTypes;

public class Create
{
    [Test]
    public async Task WhenResourceIsValid_ReturnsId()
    {
        var item = new ActionType(Guid.NewGuid(), TestConstants.ValidName);
        var repoMock = new Mock<IActionTypeRepository>();
        var managerMock = new Mock<IActionTypeManager>();
        managerMock.Setup(l => l.CreateAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(item);
        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(l => l.GetCurrentUserAsync())
            .ReturnsAsync((ApplicationUser?)null);
        var appService = new ActionTypeAppService(repoMock.Object, managerMock.Object,
            AppServicesTestsGlobal.Mapper!, userServiceMock.Object);

        var result = await appService.CreateAsync(item.Name);

        result.Should().Be(item.Id);
    }
}
