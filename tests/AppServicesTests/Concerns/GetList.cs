using Cts.AppServices.Concerns;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities.Concerns;
using Cts.TestData.Constants;

namespace AppServicesTests.Concerns;

public class GetList
{
    [Test]
    public async Task ReturnsViewDtoList()
    {
        var itemList = new List<Concern> { new(Guid.Empty, TextData.ValidName) };
        var repoMock = Substitute.For<IConcernRepository>();
        repoMock.GetOrderedListAsync(Arg.Any<CancellationToken>())
            .Returns(itemList);
        var managerMock = Substitute.For<IConcernManager>();
        var userServiceMock = Substitute.For<IUserService>();
        var appService = new ConcernService(repoMock, managerMock,
            AppServicesTestsSetup.Mapper!, userServiceMock);

        var result = await appService.GetListAsync();

        result.Should().BeEquivalentTo(itemList);
    }
}
