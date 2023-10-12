using Cts.AppServices.Offices;
using Cts.AppServices.Staff;
using Cts.AppServices.Staff.Dto;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities.Offices;
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
        List<OfficeWithAssignorDto> officeList = new()
            { new OfficeWithAssignorDto(Guid.Empty, TextData.ValidName, true) };
        var officeServiceMock = Substitute.For<IOfficeService>();
        officeServiceMock.GetListAsync(CancellationToken.None).Returns(officeList);
        var staffServiceMock = Substitute.For<IStaffService>();
        var userServiceMock = Substitute.For<IUserService>();
        var apiController = new OfficeApiController(officeServiceMock, staffServiceMock, userServiceMock);

        var result = await apiController.ListOfficesAsync();

        result.Should().BeEquivalentTo(officeList);
    }

    [Test]
    public async Task GetStaffForAssignment_GivenUserIsInOffice_ReturnsWithList()
    {
        // Arrange
        var officeMock = Substitute.For<IOfficeService>();
        officeMock.GetStaffListItemsAsync(Arg.Any<Guid?>(), Arg.Any<bool>(), Arg.Any<CancellationToken>())
            .Returns(ListItems);

        var staffMock = Substitute.For<IStaffService>();
        staffMock.GetCurrentUserAsync().Returns(new StaffViewDto
            { Active = true, Office = new OfficeViewDto(Guid.Empty, TextData.ValidName, true) });

        var userMock = Substitute.For<IUserService>();
        userMock.GetCurrentUserAsync()
            .Returns(new ApplicationUser { Office = new Office(Guid.Empty, TextData.ValidName) });

        var controller = new OfficeApiController(officeMock, staffMock, userMock);

        // Act
        var response = await controller.GetStaffForAssignmentAsync(Guid.Empty);

        // Assert
        ((JsonResult)response).Value.Should().BeEquivalentTo(ListItems);
    }

    [Test]
    public async Task GetStaffForAssignment_GivenUserIsAssignor_ReturnsWithList()
    {
        // Arrange
        var officeMock = Substitute.For<IOfficeService>();
        officeMock.GetStaffListItemsAsync(Arg.Any<Guid?>(), Arg.Any<bool>(), Arg.Any<CancellationToken>())
            .Returns(ListItems);
        officeMock.UserIsAssignorAsync(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(true);

        var staffMock = Substitute.For<IStaffService>();
        staffMock.GetCurrentUserAsync().Returns(new StaffViewDto { Active = true });

        var userMock = Substitute.For<IUserService>();
        userMock.GetCurrentUserAsync().Returns(new ApplicationUser());

        var controller = new OfficeApiController(officeMock, staffMock, userMock);

        // Act
        var response = await controller.GetStaffForAssignmentAsync(Guid.Empty);

        // Assert
        ((JsonResult)response).Value.Should().BeEquivalentTo(ListItems);
    }

    [Test]
    public async Task GetStaffForAssignment_GivenUserIsDivisionManager_ReturnsWithList()
    {
        // Arrange
        var officeMock = Substitute.For<IOfficeService>();
        officeMock.GetStaffListItemsAsync(Arg.Any<Guid?>(), Arg.Any<bool>(), Arg.Any<CancellationToken>())
            .Returns(ListItems);

        var staffMock = Substitute.For<IStaffService>();
        staffMock.GetCurrentUserAsync().Returns(new StaffViewDto { Active = true });
        staffMock.HasAppRoleAsync(Arg.Any<string>(), Arg.Any<AppRole>()).Returns(true);

        var userMock = Substitute.For<IUserService>();
        userMock.GetCurrentUserAsync().Returns(new ApplicationUser());

        var controller = new OfficeApiController(officeMock, staffMock, userMock);

        // Act
        var response = await controller.GetStaffForAssignmentAsync(Guid.Empty);

        // Assert
        ((JsonResult)response).Value.Should().BeEquivalentTo(ListItems);
    }

    [Test]
    public async Task GetStaffForAssignment_GivenNoAuthenticatedUser_ReturnsUnauthorized()
    {
        // Arrange
        var staffMock = Substitute.For<IStaffService>();
        var userMock = Substitute.For<IUserService>();
        userMock.GetCurrentUserAsync().Returns((ApplicationUser?)null);
        var controller = new OfficeApiController(Substitute.For<IOfficeService>(), staffMock, userMock);

        // Act
        var response = await controller.GetStaffForAssignmentAsync(Guid.Empty);

        // Assert
        using var scope = new AssertionScope();
        response.Should().BeOfType<UnauthorizedResult>();
        ((UnauthorizedResult)response).StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
    }

    [Test]
    public async Task GetStaffForAssignment_GivenUserIsInactive_ReturnsForbidden()
    {
        // Arrange
        var staffMock = Substitute.For<IStaffService>();
        staffMock.GetCurrentUserAsync().Returns(new StaffViewDto { Active = false });

        var userMock = Substitute.For<IUserService>();
        userMock.GetCurrentUserAsync().Returns(new ApplicationUser());

        var controller = new OfficeApiController(Substitute.For<IOfficeService>(), staffMock, userMock);

        // Act
        var response = await controller.GetStaffForAssignmentAsync(Guid.Empty);

        // Assert
        using var scope = new AssertionScope();
        response.Should().BeOfType<ObjectResult>();
        ((ObjectResult)response).StatusCode.Should().Be(StatusCodes.Status403Forbidden);
        ((ProblemDetails)((ObjectResult)response).Value!).Detail.Should().Be("Forbidden");
    }

    [Test]
    public async Task GetStaffForAssignment_GivenUserDoesNotHavePermission_ReturnsForbidden()
    {
        // Arrange
        var officeMock = Substitute.For<IOfficeService>();
        officeMock.GetStaffListItemsAsync(Arg.Any<Guid?>(), Arg.Any<bool>(), Arg.Any<CancellationToken>())
            .Returns(ListItems);
        officeMock.UserIsAssignorAsync(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(false);

        var staffMock = Substitute.For<IStaffService>();
        staffMock.GetCurrentUserAsync().Returns(new StaffViewDto { Active = true });
        staffMock.HasAppRoleAsync(Arg.Any<string>(), Arg.Any<AppRole>()).Returns(false);

        var userMock = Substitute.For<IUserService>();
        userMock.GetCurrentUserAsync().Returns(new ApplicationUser());

        var controller = new OfficeApiController(Substitute.For<IOfficeService>(), staffMock, userMock);

        // Act
        var response = await controller.GetStaffForAssignmentAsync(Guid.Empty);

        // Assert
        using var scope = new AssertionScope();
        response.Should().BeOfType<ObjectResult>();
        ((ObjectResult)response).StatusCode.Should().Be(StatusCodes.Status403Forbidden);
        ((ProblemDetails)((ObjectResult)response).Value!).Detail.Should().Be("Forbidden");
    }
}
