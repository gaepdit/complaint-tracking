using Cts.AppServices.Concerns;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities.Concerns;
using Cts.TestData.Constants;
using System.Linq.Expressions;

namespace AppServicesTests.Concerns;

public class GetActiveListItems
{
    [Test]
    public async Task ReturnsActiveListItems()
    {
        var itemList = new List<Concern> { new(Guid.Empty, TestConstants.ValidName) };
        var repoMock = new Mock<IConcernRepository>();
        repoMock.Setup(l =>
                l.GetListAsync(It.IsAny<Expression<Func<Concern, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(itemList);
        var managerMock = new Mock<IConcernManager>();
        var userServiceMock = new Mock<IUserService>();
        var appService = new ConcernAppService(repoMock.Object, managerMock.Object,
            AppServicesTestsGlobal.Mapper!, userServiceMock.Object);

        var result = await appService.GetActiveListItemsAsync();

        result.Should().BeEquivalentTo(itemList);
    }
}
