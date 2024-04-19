using Cts.AppServices.Offices;
using Cts.WebApp.Api;
using Microsoft.AspNetCore.Http;

namespace WebAppTests.Api;

[TestFixture]
public class OfficeApiTests
{
    private static readonly List<ListItem<string>> ListItems =
        [new ListItem<string>(Guid.Empty.ToString(), TextData.ValidName)];

    [Test]
    public async Task ListOffices_ReturnsListOfOffices()
    {
        // Arrange
        List<OfficeWithAssignorDto> officeList = [new OfficeWithAssignorDto(Guid.Empty, TextData.ValidName, true)];

        var officeServiceMock = Substitute.For<IOfficeService>();
        officeServiceMock.GetListIncludeAssignorAsync(CancellationToken.None).Returns(officeList);

        var apiController = new OfficeApiController(officeServiceMock, Substitute.For<IAuthorizationService>());

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

        var authorizationMock = Substitute.For<IAuthorizationService>();
        authorizationMock.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), resource: Arg.Any<object?>(),
                requirements: Arg.Any<IEnumerable<IAuthorizationRequirement>>())
            .Returns(AuthorizationResult.Success());

        var controller = new OfficeApiController(officeMock, authorizationMock);

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

        var controller = new OfficeApiController(Substitute.For<IOfficeService>(), authorizationMock);

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
        var officeServiceMock = Substitute.For<IOfficeService>();
        officeServiceMock.FindAsync(Arg.Any<Guid>())
            .Returns(new OfficeWithAssignorDto(Guid.Empty, TextData.ValidName, true));

        var authorizationMock = Substitute.For<IAuthorizationService>();
        // This returns success for line 39: User is active user.
        authorizationMock.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), resource: Arg.Is<object?>(x => x == null),
                requirements: Arg.Any<IEnumerable<IAuthorizationRequirement>>())
            .Returns(AuthorizationResult.Success());
        // This returns failure for line 43: User does not meet OfficeAssignmentRequirement.
        authorizationMock.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), resource: Arg.Is<object?>(x => x != null),
                requirements: Arg.Any<IEnumerable<IAuthorizationRequirement>>())
            .Returns(AuthorizationResult.Failed());

        var controller = new OfficeApiController(officeServiceMock, authorizationMock);

        // Act
        var response = await controller.GetStaffForAssignmentAsync(Guid.Empty);

        // Assert
        ((JsonResult)response).Value.Should().BeNull();
    }
}
