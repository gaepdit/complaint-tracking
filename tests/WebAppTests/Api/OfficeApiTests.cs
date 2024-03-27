using Cts.AppServices.Offices;
using Cts.AppServices.UserServices;
using Cts.Domain.Identity;
using Cts.WebApp.Api;
using Microsoft.AspNetCore.Http;

namespace WebAppTests.Api;

[TestFixture]
public class OfficeApiTests
{
    private static readonly List<ListItem<string>> ListItems = new()
        { new ListItem<string>(Guid.Empty.ToString(), TextData.ValidName) };

    [Test]
    public async Task ListOffices_ReturnsListOfOffices()
    {
        // Arrange
        List<OfficeWithAssignorDto> officeList = [new OfficeWithAssignorDto(Guid.Empty, TextData.ValidName, true)];

        var officeServiceMock = Substitute.For<IOfficeService>();
        officeServiceMock.GetListIncludeAssignorAsync(CancellationToken.None).Returns(officeList);

        var apiController = new OfficeApiController(officeServiceMock, Substitute.For<IUserService>(),
            Substitute.For<IAuthorizationService>());

        // Act
        var result = await apiController.ListOfficesAsync();

        // Assert
        result.Should().BeEquivalentTo(officeList);
    }

    [Test]
    public async Task GetStaffForAssignment_GivenUserIsAuthorized_ReturnsWithList()
    {
        // Arrange
        var officeMock = Substitute.For<IOfficeService>();
        officeMock.GetStaffAsListItemsAsync(Arg.Any<Guid?>(), Arg.Any<bool>(), Arg.Any<CancellationToken>())
            .Returns(ListItems);

        var userMock = Substitute.For<IUserService>();
        userMock.GetCurrentUserAsync().Returns(new ApplicationUser());

        var authorizationMock = Substitute.For<IAuthorizationService>();
        authorizationMock.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), resource: Arg.Any<object?>(),
                requirements: Arg.Any<IEnumerable<IAuthorizationRequirement>>())
            .Returns(AuthorizationResult.Success());

        var controller = new OfficeApiController(officeMock, userMock, authorizationMock);

        // Act
        var response = await controller.GetStaffForAssignmentAsync(Guid.Empty);

        // Assert
        ((JsonResult)response).Value.Should().BeEquivalentTo(ListItems);
    }

    [Test]
    public async Task GetStaffForAssignment_GivenNoAuthenticatedUser_ReturnsUnauthorized()
    {
        // Arrange
        var authorizationMock = Substitute.For<IAuthorizationService>();
        authorizationMock.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), resource: Arg.Any<object?>(),
                requirements: Arg.Any<IEnumerable<IAuthorizationRequirement>>())
            .Returns(AuthorizationResult.Failed());

        var controller = new OfficeApiController(Substitute.For<IOfficeService>(), Substitute.For<IUserService>(),
            authorizationMock);

        // Act
        var response = await controller.GetStaffForAssignmentAsync(Guid.Empty);

        // Assert
        using var scope = new AssertionScope();
        response.Should().BeOfType<UnauthorizedResult>();
        ((UnauthorizedResult)response).StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
    }

    [Test]
    public async Task GetStaffForAssignment_GivenUserDoesNotHaveAccess_ReturnsEmptyList()
    {
        // Arrange
        var officeMock = Substitute.For<IOfficeService>();
        officeMock.GetStaffAsListItemsAsync(Arg.Any<Guid?>(), Arg.Any<bool>(), Arg.Any<CancellationToken>())
            .Returns(ListItems);
        officeMock.UserIsAssignorForOfficeAsync(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(false);

        var userMock = Substitute.For<IUserService>();
        userMock.GetCurrentUserAsync().Returns(new ApplicationUser());

        var authorizationMock = Substitute.For<IAuthorizationService>();
        // This returns success for line 40: User is active user.
        authorizationMock.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), resource: Arg.Is<object?>(x => x == null),
                requirements: Arg.Any<IEnumerable<IAuthorizationRequirement>>())
            .Returns(AuthorizationResult.Success());
        // This returns failure for line 45: User does not meet OfficeAssignmentRequirement.
        authorizationMock.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), resource: Arg.Is<object?>(x => x != null),
                requirements: Arg.Any<IEnumerable<IAuthorizationRequirement>>())
            .Returns(AuthorizationResult.Failed());

        var controller = new OfficeApiController(Substitute.For<IOfficeService>(), userMock, authorizationMock);

        // Act
        var response = await controller.GetStaffForAssignmentAsync(Guid.Empty);

        // Assert
        ((JsonResult)response).Value.Should().BeNull();
    }
}
