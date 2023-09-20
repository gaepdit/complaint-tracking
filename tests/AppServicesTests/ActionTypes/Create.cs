using Cts.AppServices.ActionTypes;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities.ActionTypes;
using Cts.Domain.Identity;
using Cts.TestData.Constants;

namespace AppServicesTests.ActionTypes;

public class Create
{
    [Test]
    public async Task WhenResourceIsValid_ReturnsId()
    {
        var item = new ActionType(Guid.NewGuid(), TextData.ValidName);
        var repoMock = Substitute.For<IActionTypeRepository>();
        var managerMock = Substitute.For<IActionTypeManager>();
        managerMock.CreateAsync(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<CancellationToken>())
            .Returns(item);
        var userServiceMock = Substitute.For<IUserService>();
        userServiceMock.GetCurrentUserAsync()
            .Returns((ApplicationUser?)null);
        var appService = new ActionTypeService(repoMock, managerMock,
            AppServicesTestsSetup.Mapper!, userServiceMock);

        var result = await appService.CreateAsync(item.Name);

        result.Should().Be(item.Id);
    }
}
