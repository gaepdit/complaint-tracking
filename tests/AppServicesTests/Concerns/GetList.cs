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
        var itemList = new List<Concern> { new(Guid.Empty, TestConstants.ValidName) };
        var repoMock = new Mock<IConcernRepository>();
        repoMock.Setup(l => l.GetListAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(itemList);
        var managerMock = new Mock<IConcernManager>();
        var userServiceMock = new Mock<IUserService>();
        var appService = new ConcernAppService(repoMock.Object, managerMock.Object,
            AppServicesTestsSetup.Mapper!, userServiceMock.Object);

        var result = await appService.GetListAsync();

        result.Should().BeEquivalentTo(itemList);
    }
}
