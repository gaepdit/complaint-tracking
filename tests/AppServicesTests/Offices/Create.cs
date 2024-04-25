using Cts.AppServices.Offices;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities.Offices;
using Cts.Domain.Identity;
using Cts.TestData.Constants;
using Microsoft.AspNetCore.Authorization;

namespace AppServicesTests.Offices;

public class Create
{
    [Test]
    public async Task WhenResourceIsValid_ReturnsId()
    {
        // Arrange
        var item = new Office(Guid.NewGuid(), TextData.ValidName);

        var managerMock = Substitute.For<IOfficeManager>();
        managerMock.CreateAsync(Arg.Any<string>(), Arg.Is((string?)null), Arg.Any<CancellationToken>()).Returns(item);
        
        var userServiceMock = Substitute.For<IUserService>();
        userServiceMock.GetCurrentUserAsync().Returns((ApplicationUser?)null);
        
        var appService = new OfficeService(Substitute.For<IOfficeRepository>(), managerMock, AppServicesTestsSetup.Mapper!, userServiceMock,
            Substitute.For<IAuthorizationService>());
        
        var resource = new OfficeCreateDto(TextData.ValidName);

        // Act
        var result = await appService.CreateAsync(resource);

        // Assert
        result.Should().Be(item.Id);
    }
}
