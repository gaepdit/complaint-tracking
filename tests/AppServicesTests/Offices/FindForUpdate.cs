using AutoMapper;
using Cts.AppServices.Offices;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities.Offices;
using Cts.Domain.Identity;
using Cts.TestData.Constants;
using Microsoft.AspNetCore.Authorization;

namespace AppServicesTests.Offices;

public class FindForUpdate
{
    [Test]
    public async Task WhenItemExists_ReturnsViewDto()
    {
        // Arrange
        var user = new ApplicationUser
        {
            Id = Guid.NewGuid().ToString(),
            GivenName = TextData.ValidName,
            FamilyName = TextData.NewValidName,
            Email = TextData.ValidEmail,
        };
        var office = new Office(Guid.Empty, TextData.ValidName) { Assignor = user };

        var repoMock = Substitute.For<IOfficeRepository>();
        repoMock.FindIncludeAssignorAsync(office.Id).Returns(office);

        var appService = new OfficeService(repoMock, Substitute.For<IOfficeManager>(), AppServicesTestsSetup.Mapper!,
            Substitute.For<IUserService>(), Substitute.For<IAuthorizationService>());

        // Act
        var result = await appService.FindForUpdateAsync(Guid.Empty);

        // Assert
        result.Should().BeEquivalentTo(office);
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsNull()
    {
        // Arrange
        var repoMock = Substitute.For<IOfficeRepository>();
        repoMock.FindAsync(Arg.Any<Guid>()).Returns((Office?)null);

        var appService = new OfficeService(repoMock, Substitute.For<IOfficeManager>(), Substitute.For<IMapper>(),
            Substitute.For<IUserService>(), Substitute.For<IAuthorizationService>());

        // Act
        var result = await appService.FindForUpdateAsync(Guid.Empty);

        // Assert
        result.Should().BeNull();
    }
}
