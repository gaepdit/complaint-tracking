using Cts.AppServices.ActionTypes;
using Cts.Domain.ActionTypes;
using Cts.TestData.ActionTypes;

namespace AppServicesTests.ActionTypes;

public class GetList
{
    [Test]
    public async Task WhenItemExists_ReturnsViewDto()
    {
        var item = new List<ActionType> { new(Guid.Empty, ActionTypeConstants.ValidName) };
        var repoMock = new Mock<IActionTypeRepository>();
        repoMock.Setup(l => l.GetListAsync(default))
            .ReturnsAsync(item);
        var managerMock = new Mock<IActionTypeManager>();
        var appService = new ActionTypeAppService(repoMock.Object, managerMock.Object, AppServicesTestsGlobal.Mapper!);

        var result = await appService.GetListAsync();

        result.Should().BeEquivalentTo(item);
    }
}
