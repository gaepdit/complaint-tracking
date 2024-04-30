using Cts.AppServices.ActionTypes;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities.ActionTypes;
using Cts.TestData.Constants;

namespace AppServicesTests.ActionTypes;

public class GetList
{
    [Test]
    public async Task WhenItemsExist_ReturnsList()
    {
        var itemList = new List<ActionType> { new(Guid.Empty, TextData.ValidName) };
        var repoMock = Substitute.For<IActionTypeRepository>();
        repoMock.GetOrderedListAsync(Arg.Any<CancellationToken>())
            .Returns(itemList);
        var managerMock = Substitute.For<IActionTypeManager>();
        var userServiceMock = Substitute.For<IUserService>();
        var appService = new ActionTypeService(repoMock, managerMock,
            AppServicesTestsSetup.Mapper!, userServiceMock);

        var result = await appService.GetListAsync();

        result.Should().BeEquivalentTo(itemList);
    }
}
