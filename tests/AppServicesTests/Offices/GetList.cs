using Cts.AppServices.Offices;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities;
using Cts.Domain.Offices;
using Cts.TestData.Constants;

namespace AppServicesTests.Offices;

public class GetList
{
    [Test]
    public async Task WhenItemsExist_ReturnsViewDtoList()
    {
        var office = new Office(Guid.Empty, TestConstants.ValidName);
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
        userServiceMock.Setup(l => l.GetCurrentUserAsync())
            .ReturnsAsync((ApplicationUser?)null);
        var appService = new OfficeAppService(repoMock.Object, managerMock.Object,
            AppServicesTestsGlobal.Mapper!, userServiceMock.Object);

        var result = await appService.GetListAsync();

        Assert.Multiple(() =>
        {
            result.Should().BeEquivalentTo(itemList, options => options
                .Excluding(ctx => ctx.Path.EndsWith("MasterUser.Id")));
            result[0].MasterUser!.Id.ToString().Should().Be(office.MasterUser.Id);
        });
    }
}
