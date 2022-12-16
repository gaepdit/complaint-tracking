using Cts.AppServices.Concerns;
using Cts.AppServices.UserServices;
using Cts.Domain.Concerns;
using Cts.Domain.Identity;
using Cts.TestData.Constants;

namespace AppServicesTests.Concerns;

public class GetList
{
    [Test]
    public async Task WhenItemsExist_ReturnsViewDtoList()
    {
        var itemList = new List<Concern> { new(Guid.Empty, TestConstants.ValidName) };
        var repoMock = new Mock<IConcernRepository>();
        repoMock.Setup(l => l.GetListAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(itemList);
        var managerMock = new Mock<IConcernManager>();
        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(l => l.GetCurrentUserAsync())
            .ReturnsAsync((ApplicationUser?)null);
        var appService = new ConcernAppService(repoMock.Object, managerMock.Object,
            AppServicesTestsGlobal.Mapper!, userServiceMock.Object);

        var result = await appService.GetListAsync();

        result.Should().BeEquivalentTo(itemList);
    }
}
