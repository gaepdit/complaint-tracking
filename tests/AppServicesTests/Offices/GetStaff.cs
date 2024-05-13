using Cts.AppServices.Offices;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities.Offices;
using Cts.Domain.Identity;
using Cts.TestData.Constants;
using Microsoft.AspNetCore.Authorization;

namespace AppServicesTests.Offices;

public class GetStaff
{
    [Test]
    public async Task WhenOfficeExists_ReturnsViewDtoList()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var user = new ApplicationUser
        {
            Id = Guid.Empty.ToString(),
            GivenName = TextData.ValidName,
            FamilyName = TextData.NewValidName,
            Email = TextData.ValidEmail,
            Active = false,
        };

        var itemList = new List<ApplicationUser> { user };

        var repoMock = Substitute.For<IOfficeRepository>();
        repoMock.GetStaffMembersListAsync(guid, false, Arg.Any<CancellationToken>())
            .Returns(itemList);

        var appService = new OfficeService(repoMock, Substitute.For<IOfficeManager>(), AppServicesTestsSetup.Mapper!,
            Substitute.For<IUserService>(), Substitute.For<IAuthorizationService>());

        // Act
        var result = await appService.GetStaffAsListItemsAsync(guid);

        // Assert
        result.Should().ContainSingle(e =>
            string.Equals(e.Id, user.Id, StringComparison.Ordinal) &&
            string.Equals(e.Name, user.SortableNameWithInactive, StringComparison.Ordinal));
    }
}
