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
        var itemList = new List<Concern> { new(Guid.Empty, TextData.ValidName) };
        var repoMock = Substitute.For<IConcernRepository>();
        repoMock.GetListAsync(Arg.Any<Expression<Func<Concern, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(itemList);
        var managerMock = Substitute.For<IConcernManager>();
        var userServiceMock = Substitute.For<IUserService>();
        var appService = new ConcernService(repoMock, managerMock,
            AppServicesTestsSetup.Mapper!, userServiceMock);

        var result = await appService.GetActiveListItemsAsync();

        result.Should().BeEquivalentTo(itemList);
    }
}
