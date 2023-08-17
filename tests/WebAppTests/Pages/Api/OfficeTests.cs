using Cts.AppServices.Offices;
using Cts.AppServices.Staff;
using Cts.AppServices.Staff.Dto;
using Cts.Domain.Identity;
using Cts.TestData.Constants;
using Cts.WebApp.Api;
using FluentAssertions.Execution;
using GaEpd.AppLibrary.ListItems;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAppTests.Api;

public class OfficeTests
{
    private static readonly List<ListItem<string>> ListItems = new()
        { new ListItem<string>(Guid.Empty.ToString(), TestConstants.ValidName) };

    [Test]
    public async Task GetStaffForAssignment_GivenUserIsInOffice_ReturnsWithList()
    {
        // Arrange
        var officeMock = Substitute.For<IOfficeService>();
        officeMock.GetStaffListItemsAsync(Arg.Any<Guid?>(), Arg.Any<bool>(), Arg.Any<CancellationToken>())
            .Returns(ListItems);

        var staffMock = Substitute.For<IStaffService>();
        staffMock.FindCurrentUserAsync()
            .Returns(new StaffViewDto { Active = true, Office = new OfficeDisplayViewDto { Id = Guid.Empty } });

        var controller = new OfficeApiController(officeMock, staffMock);

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
        officeMock.UserIsAssignorAsync(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(true);

        var staffMock = Substitute.For<IStaffService>();
        staffMock.FindCurrentUserAsync()
            .Returns(new StaffViewDto { Active = true });

        var controller = new OfficeApiController(officeMock, staffMock);

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
        staffMock.FindCurrentUserAsync()
            .Returns(new StaffViewDto { Active = true });
        staffMock.HasAppRoleAsync(Arg.Any<string>(), Arg.Any<AppRole>())
            .Returns(true);

        var controller = new OfficeApiController(officeMock, staffMock);

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
        staffMock.FindCurrentUserAsync()
            .Returns((StaffViewDto?)null);

        var controller = new OfficeApiController(Substitute.For<IOfficeService>(), staffMock);

        // Act
        var response = await controller.GetStaffForAssignmentAsync(Guid.Empty);

        // Assert
        using (new AssertionScope())
        {
            response.Should().BeOfType<UnauthorizedResult>();
            ((UnauthorizedResult)response).StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }
    }

    [Test]
    public async Task GetStaffForAssignment_GivenUserIsInactive_ReturnsForbidden()
    {
        // Arrange
        var staffMock = Substitute.For<IStaffService>();
        staffMock.FindCurrentUserAsync()
            .Returns(new StaffViewDto { Active = false });

        var controller = new OfficeApiController(Substitute.For<IOfficeService>(), staffMock);

        // Act
        var response = await controller.GetStaffForAssignmentAsync(Guid.Empty);

        // Assert
        using (new AssertionScope())
        {
            response.Should().BeOfType<ObjectResult>();
            ((ObjectResult)response).StatusCode.Should().Be(StatusCodes.Status403Forbidden);
            ((ProblemDetails)((ObjectResult)response).Value!).Detail.Should().Be("Forbidden");
        }
    }

    [Test]
    public async Task GetStaffForAssignment_GivenUserDoesNotHavePermission_ReturnsForbidden()
    {
        // Arrange
        var officeMock = Substitute.For<IOfficeService>();
        officeMock.GetStaffListItemsAsync(Arg.Any<Guid?>(), Arg.Any<bool>(), Arg.Any<CancellationToken>())
            .Returns(ListItems);
        officeMock.UserIsAssignorAsync(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(false);

        var staffMock = Substitute.For<IStaffService>();
        staffMock.FindCurrentUserAsync()
            .Returns(new StaffViewDto { Active = true });
        staffMock.HasAppRoleAsync(Arg.Any<string>(), Arg.Any<AppRole>())
            .Returns(false);

        var controller = new OfficeApiController(Substitute.For<IOfficeService>(), staffMock);

        // Act
        var response = await controller.GetStaffForAssignmentAsync(Guid.Empty);

        // Assert
        using (new AssertionScope())
        {
            response.Should().BeOfType<ObjectResult>();
            ((ObjectResult)response).StatusCode.Should().Be(StatusCodes.Status403Forbidden);
            ((ProblemDetails)((ObjectResult)response).Value!).Detail.Should().Be("Forbidden");
        }
    }
}
