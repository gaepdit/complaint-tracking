using MyAppRoot.AppServices.Offices;
using MyAppRoot.AppServices.UserServices;
using MyAppRoot.Domain.Identity;
using MyAppRoot.Domain.Offices;
using MyAppRoot.TestData.Constants;

namespace AppServicesTests.Offices;

public class GetActiveStaff
{
    [Test]
    public async Task WhenOfficeExists_ReturnsViewDtoList()
    {
        var user = new ApplicationUser
        {
            FirstName = TestConstants.ValidName,
            LastName = TestConstants.NewValidName,
            Email = TestConstants.ValidEmail,
        };

        var itemList = new List<ApplicationUser> { user };
        var repoMock = new Mock<IOfficeRepository>();
        repoMock.Setup(l => l.GetActiveStaffMembersListAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(itemList);
        var managerMock = new Mock<IOfficeManager>();
        var userServiceMock = new Mock<IUserService>();

        var appService = new OfficeAppService(repoMock.Object, managerMock.Object,
            AppServicesTestsGlobal.Mapper!, userServiceMock.Object);

        var result = await appService.GetActiveStaffAsync(Guid.Empty);

        result.Should().ContainSingle(e =>
            e.FirstName == user.FirstName && e.LastName == user.LastName && e.Email == user.Email);
    }
}
