using Cts.AppServices.Offices;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities.Offices;
using Cts.Domain.Identity;
using Cts.TestData.Constants;
using Microsoft.AspNetCore.Authorization;

namespace AppServicesTests.Offices;

public class GetList
{
    [Test]
    public async Task WhenItemsExist_ReturnsViewDtoList()
    {
        // Arrange
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

        var appService = new OfficeService(repoMock, Substitute.For<IOfficeManager>(), AppServicesTestsSetup.Mapper!,
            Substitute.For<IUserService>(), Substitute.For<IAuthorizationService>());

        // Act
        var result = await appService.GetListIncludeAssignorAsync();

        // Assert
        result.Should().BeEquivalentTo(itemList);
    }

    [Test]
    public async Task WhenNoItemsExist_ReturnsEmptyList()
    {
        // Arrange
        var repoMock = Substitute.For<IOfficeRepository>();
        repoMock.GetListAsync().Returns(new List<Office>());

        var appService = new OfficeService(repoMock, Substitute.For<IOfficeManager>(), AppServicesTestsSetup.Mapper!,
            Substitute.For<IUserService>(), Substitute.For<IAuthorizationService>());

        // Act
        var result = await appService.GetListAsync();

        // Assert
        result.Should().BeEmpty();
    }
}
