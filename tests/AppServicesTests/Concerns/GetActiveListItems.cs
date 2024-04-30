using Cts.AppServices.Concerns;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities.Concerns;
using System.Linq.Expressions;

namespace AppServicesTests.Concerns;

public class GetActiveListItems
{
    [Test]
    public async Task GetAsListItems_ReturnsListOfListItems()
    {
        // Arrange
        var itemList = new List<Concern>
        {
            new(Guid.Empty, "One"),
            new(Guid.Empty, "Two"),
        };

        var repoMock = Substitute.For<IConcernRepository>();
        repoMock.GetOrderedListAsync(Arg.Any<Expression<Func<Concern, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(itemList);

        var managerMock = Substitute.For<IConcernManager>();
        var userServiceMock = Substitute.For<IUserService>();
        var appService = new ConcernService(repoMock, managerMock, AppServicesTestsSetup.Mapper!, userServiceMock);

        // Act
        var result = await appService.GetAsListItemsAsync();

        // Assert
        result.Should().BeEquivalentTo(itemList);
    }
}
