using Cts.AppServices.ActionTypes;
using Cts.Domain.ActionTypes;
using Cts.TestData.ActionTypes;

namespace AppServicesTests.ActionTypes;

public class Create
{
    [Test]
    public async Task WhenResourceIsValid_ReturnsViewDto()
    {
        var item = new ActionType(Guid.NewGuid(), ActionTypeConstants.ValidName);
        var repoMock = new Mock<IActionTypeRepository>();
        var managerMock = new Mock<IActionTypeManager>();
        managerMock.Setup(l => l.CreateAsync(It.IsAny<string>()))
            .ReturnsAsync(item);
        var appService = new ActionTypeAppService(repoMock.Object, managerMock.Object, AppServicesTestsGlobal.Mapper!);

        var result = await appService.CreateAsync(item.Name);

        result.Should().BeEquivalentTo(item);
    }
}
