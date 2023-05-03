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
        var officeMock = new Mock<IOfficeService>();
        officeMock.Setup(l =>
                l.GetStaffListItemsAsync(It.IsAny<Guid?>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ListItems);

        var staffMock = new Mock<IStaffService>();
        staffMock.Setup(l => l.FindCurrentUserAsync())
            .ReturnsAsync(new StaffViewDto { Active = true, Office = new OfficeDisplayViewDto { Id = Guid.Empty } });

        var controller = new OfficeApiController(officeMock.Object, staffMock.Object);

        // Act
        var response = await controller.GetStaffForAssignmentAsync(Guid.Empty);

        // Assert
        ((JsonResult)response).Value.Should().BeEquivalentTo(ListItems);
    }

    [Test]
    public async Task GetStaffForAssignment_GivenUserIsAssignor_ReturnsWithList()
    {
        // Arrange
        var officeMock = new Mock<IOfficeService>();
        officeMock.Setup(l =>
                l.GetStaffListItemsAsync(It.IsAny<Guid?>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ListItems);
        officeMock.Setup(
                l => l.UserIsAssignorAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var staffMock = new Mock<IStaffService>();
        staffMock.Setup(l => l.FindCurrentUserAsync())
            .ReturnsAsync(new StaffViewDto { Active = true });

        var controller = new OfficeApiController(officeMock.Object, staffMock.Object);

        // Act
        var response = await controller.GetStaffForAssignmentAsync(Guid.Empty);

        // Assert
        ((JsonResult)response).Value.Should().BeEquivalentTo(ListItems);
    }

    [Test]
    public async Task GetStaffForAssignment_GivenUserIsDivisionManager_ReturnsWithList()
    {
        // Arrange
        var officeMock = new Mock<IOfficeService>();
        officeMock.Setup(l =>
                l.GetStaffListItemsAsync(It.IsAny<Guid?>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ListItems);

        var staffMock = new Mock<IStaffService>();
        staffMock.Setup(l => l.FindCurrentUserAsync())
            .ReturnsAsync(new StaffViewDto { Active = true });
        staffMock.Setup(l => l.HasAppRoleAsync(It.IsAny<string>(), It.IsAny<AppRole>()))
            .ReturnsAsync(true);

        var controller = new OfficeApiController(officeMock.Object, staffMock.Object);

        // Act
        var response = await controller.GetStaffForAssignmentAsync(Guid.Empty);

        // Assert
        ((JsonResult)response).Value.Should().BeEquivalentTo(ListItems);
    }

    [Test]
    public async Task GetStaffForAssignment_GivenNoAuthenticatedUser_ReturnsUnauthorized()
    {
        // Arrange
        var staffMock = new Mock<IStaffService>();
        staffMock.Setup(l => l.FindCurrentUserAsync())
            .ReturnsAsync((StaffViewDto?)null);

        var controller = new OfficeApiController(Mock.Of<IOfficeService>(), staffMock.Object);

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
        var staffMock = new Mock<IStaffService>();
        staffMock.Setup(l => l.FindCurrentUserAsync())
            .ReturnsAsync(new StaffViewDto { Active = false });

        var controller = new OfficeApiController(Mock.Of<IOfficeService>(), staffMock.Object);

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
        var officeMock = new Mock<IOfficeService>();
        officeMock.Setup(l =>
                l.GetStaffListItemsAsync(It.IsAny<Guid?>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ListItems);
        officeMock.Setup(
                l => l.UserIsAssignorAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var staffMock = new Mock<IStaffService>();
        staffMock.Setup(l => l.FindCurrentUserAsync())
            .ReturnsAsync(new StaffViewDto { Active = true });
        staffMock.Setup(l => l.HasAppRoleAsync(It.IsAny<string>(), It.IsAny<AppRole>()))
            .ReturnsAsync(false);

        var controller = new OfficeApiController(Mock.Of<IOfficeService>(), staffMock.Object);

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
