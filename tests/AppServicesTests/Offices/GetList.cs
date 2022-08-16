using Cts.AppServices.Offices;
using Cts.AppServices.Users;
using Cts.Domain.Entities;
using Cts.Domain.Offices;
using Cts.Domain.Users;
using Cts.TestData.Offices;

namespace AppServicesTests.Offices;

public class GetList
{
    [Test]
    public async Task WhenItemsExist_ReturnsViewDtoList()
    {
        var office = new Office(Guid.Empty, OfficeConstants.ValidName);
        var user = new ApplicationUser
        {
            Id = Guid.NewGuid().ToString(),
            FirstName = "Local",
            LastName = "User",
            Email = "local.user@example.net",
            UserName = "local.user@example.net",
            NormalizedUserName = "local.user@example.net".ToUpperInvariant(),
        };
        office.MasterUser = user;
        var itemList = new List<Office> { office };
        
        var repoMock = new Mock<IOfficeRepository>();
        repoMock.Setup(l => l.GetListAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(itemList);
        var managerMock = new Mock<IOfficeManager>();
        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(l => l.GetCurrentUserAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync((UserViewDto?)null);
        var appService = new OfficeAppService(repoMock.Object, managerMock.Object,
            AppServicesTestsGlobal.Mapper!, userServiceMock.Object);

        var result = await appService.GetListAsync();

        result.Should().BeEquivalentTo(itemList);
    }
}
