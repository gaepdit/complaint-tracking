using Cts.AppServices.Offices;
using Cts.AppServices.UserServices;
using Cts.Domain.Identity;
using Cts.Domain.Offices;
using Cts.TestData.Constants;
using FluentAssertions.Execution;

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
            FirstName = TestConstants.ValidName,
            LastName = TestConstants.NewValidName,
            Email = TestConstants.ValidEmail,
        };
        office.MasterUser = user;
        var itemList = new List<Office> { office };

        var repoMock = new Mock<IOfficeRepository>();
        repoMock.Setup(l => l.GetListAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(itemList);
        var managerMock = new Mock<IOfficeManager>();
        var userServiceMock = new Mock<IUserService>();
        var appService = new OfficeAppService(repoMock.Object, managerMock.Object,
            AppServicesTestsGlobal.Mapper!, userServiceMock.Object);

        var result = await appService.GetListAsync();

        using (new AssertionScope())
        {
            result.Should().BeEquivalentTo(itemList, options => options
                .Excluding(ctx => ctx.Path.EndsWith("MasterUser.Id")));
            result[0].MasterUser!.Id.ToString().Should().Be(office.MasterUser.Id);
        }
    }

    [Test]
    public async Task WhenNoItemsExist_ReturnsEmptyList()
    {
        var repoMock = new Mock<IOfficeRepository>();
        repoMock.Setup(l => l.GetListAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Office>());
        var managerMock = new Mock<IOfficeManager>();
        var userServiceMock = new Mock<IUserService>();
        var appService = new OfficeAppService(repoMock.Object, managerMock.Object,
            AppServicesTestsGlobal.Mapper!, userServiceMock.Object);

        var result = await appService.GetListAsync();

        result.Should().BeEmpty();
    }
}
