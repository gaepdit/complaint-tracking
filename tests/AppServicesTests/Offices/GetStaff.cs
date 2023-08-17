using Cts.AppServices.Offices;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities.Offices;
using Cts.Domain.Identity;
using Cts.TestData.Constants;

namespace AppServicesTests.Offices;

public class GetStaff
{
    [Test]
    public async Task WhenOfficeExists_ReturnsViewDtoList()
    {
        var user = new ApplicationUser
        {
            Id = Guid.Empty.ToString(),
            GivenName = TestConstants.ValidName,
            FamilyName = TestConstants.NewValidName,
            Email = TestConstants.ValidEmail,
            Active = false,
        };

        var itemList = new List<ApplicationUser> { user };
        var repoMock = Substitute.For<IOfficeRepository>();
        repoMock.GetStaffMembersListAsync(Arg.Any<Guid>(), Arg.Any<bool>(), Arg.Any<CancellationToken>())
            .Returns(itemList);
        var managerMock = Substitute.For<IOfficeManager>();
        var userServiceMock = Substitute.For<IUserService>();

        var appService = new OfficeService(repoMock, managerMock,
            AppServicesTestsSetup.Mapper!, userServiceMock);

        var result = await appService.GetStaffListItemsAsync(Guid.Empty, false);

        result.Should().ContainSingle(e =>
            string.Equals(e.Id, user.Id, StringComparison.Ordinal) &&
            string.Equals(e.Name, user.SortableNameWithInactive, StringComparison.Ordinal));
    }
}
