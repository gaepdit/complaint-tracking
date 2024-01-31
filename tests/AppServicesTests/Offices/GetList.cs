using Cts.AppServices.Offices;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities.Offices;
using Cts.Domain.Identity;
using Cts.TestData.Constants;

namespace AppServicesTests.Offices;

public class GetList
{
    [Test]
    public async Task WhenItemsExist_ReturnsViewDtoList()
    {
        var office = new Office(Guid.Empty, TextData.ValidName);
        var user = new ApplicationUser
        {
            Id = Guid.NewGuid().ToString(),
            GivenName = TextData.ValidName,
            FamilyName = TextData.NewValidName,
            Email = TextData.ValidEmail,
        };
        office.Assignor = user;
        var itemList = new List<Office> { office };

        var repoMock = Substitute.For<IOfficeRepository>();
        repoMock.GetListIncludeAssignorAsync(Arg.Any<CancellationToken>()).Returns(itemList);
        var managerMock = Substitute.For<IOfficeManager>();
        var userServiceMock = Substitute.For<IUserService>();
        var appService = new OfficeService(repoMock, managerMock,
            AppServicesTestsSetup.Mapper!, userServiceMock);

        var result = await appService.GetListIncludeAssignorAsync();

        result.Should().BeEquivalentTo(itemList);
    }

    [Test]
    public async Task WhenNoItemsExist_ReturnsEmptyList()
    {
        var repoMock = Substitute.For<IOfficeRepository>();
        repoMock.GetListAsync(Arg.Any<CancellationToken>()).Returns(new List<Office>());
        var managerMock = Substitute.For<IOfficeManager>();
        var userServiceMock = Substitute.For<IUserService>();
        var appService = new OfficeService(repoMock, managerMock,
            AppServicesTestsSetup.Mapper!, userServiceMock);

        var result = await appService.GetListAsync();

        result.Should().BeEmpty();
    }
}
