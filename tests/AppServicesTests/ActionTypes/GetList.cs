﻿using Cts.AppServices.ActionTypes;
using Cts.AppServices.UserServices;
using Cts.Domain.ActionTypes;
using Cts.TestData.Constants;

namespace AppServicesTests.ActionTypes;

public class GetList
{
    [Test]
    public async Task WhenItemsExist_ReturnsList()
    {
        var itemList = new List<ActionType> { new(Guid.Empty, TestConstants.ValidName) };
        var repoMock = new Mock<IActionTypeRepository>();
        repoMock.Setup(l => l.GetListAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(itemList);
        var managerMock = new Mock<IActionTypeManager>();
        var userServiceMock = new Mock<IUserService>();
        var appService = new ActionTypeAppService(repoMock.Object, managerMock.Object,
            AppServicesTestsGlobal.Mapper!, userServiceMock.Object);

        var result = await appService.GetListAsync();

        result.Should().BeEquivalentTo(itemList);
    }
}
