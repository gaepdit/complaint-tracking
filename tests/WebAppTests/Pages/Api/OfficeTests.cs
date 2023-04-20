using Cts.AppServices.Offices;
using Cts.AppServices.Staff;
using Cts.Domain.Identity;
using Cts.TestData.Constants;
using Cts.WebApp.Api;
using GaEpd.AppLibrary.ListItems;

namespace WebAppTests.Api;

public class OfficeTests
{
    private static readonly List<ListItem<string>> ListItems = new()
        { new ListItem<string>(Guid.Empty.ToString(), TestConstants.ValidName) };

    [Test]
    public async Task GetStaffForAssignment_GivenUserIsInOffice_ReturnsWithList()
    {
        // Arrange
        var officeMock = new Mock<IOfficeAppService>();
        officeMock.Setup(l =>
                l.GetStaffListItemsAsync(It.IsAny<Guid?>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ListItems);

        var staffMock = new Mock<IStaffAppService>();
        staffMock.Setup(l => l.FindCurrentUserAsync())
            .ReturnsAsync(new StaffViewDto { Active = true, Office = new OfficeDisplayViewDto { Id = Guid.Empty } });

        var controller = new OfficeApiController(officeMock.Object, staffMock.Object);

        // Act
        var response = await controller.GetStaffForAssignmentAsync(Guid.Empty);

        // Assert
        response.Value.Should().BeEquivalentTo(ListItems);
    }

    [Test]
    public async Task GetStaffForAssignment_GivenUserIsAssignor_ReturnsWithList()
    {
        // Arrange
        var officeMock = new Mock<IOfficeAppService>();
        officeMock.Setup(l =>
                l.GetStaffListItemsAsync(It.IsAny<Guid?>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ListItems);
        officeMock.Setup(
                l => l.UserIsAssignorAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var staffMock = new Mock<IStaffAppService>();
        staffMock.Setup(l => l.FindCurrentUserAsync())
            .ReturnsAsync(new StaffViewDto { Active = true });

        var controller = new OfficeApiController(officeMock.Object, staffMock.Object);

        // Act
        var response = await controller.GetStaffForAssignmentAsync(Guid.Empty);

        // Assert
        response.Value.Should().BeEquivalentTo(ListItems);
    }

    [Test]
    public async Task GetStaffForAssignment_GivenUserIsDivisionManager_ReturnsWithList()
    {
        // Arrange
        var officeMock = new Mock<IOfficeAppService>();
        officeMock.Setup(l =>
                l.GetStaffListItemsAsync(It.IsAny<Guid?>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ListItems);

        var staffMock = new Mock<IStaffAppService>();
        staffMock.Setup(l => l.FindCurrentUserAsync())
            .ReturnsAsync(new StaffViewDto { Active = true });
        staffMock.Setup(l => l.HasAppRoleAsync(It.IsAny<string>(), It.IsAny<AppRole>()))
            .ReturnsAsync(true);

        var controller = new OfficeApiController(officeMock.Object, staffMock.Object);

        // Act
        var response = await controller.GetStaffForAssignmentAsync(Guid.Empty);

        // Assert
        response.Value.Should().BeEquivalentTo(ListItems);
    }

    [Test]
    public async Task GetStaffForAssignment_GivenUserIsInactive_ReturnsNull()
    {
        // Arrange
        var staffMock = new Mock<IStaffAppService>();
        staffMock.Setup(l => l.FindCurrentUserAsync())
            .ReturnsAsync(new StaffViewDto { Active = false });

        var controller = new OfficeApiController(Mock.Of<IOfficeAppService>(), staffMock.Object);

        // Act
        var response = await controller.GetStaffForAssignmentAsync(Guid.Empty);

        // Assert
        response.Value.Should().BeNull();
    }

    [Test]
    public async Task GetStaffForAssignment_GivenUserDoesNotHavePermission_ReturnsNull()
    {
        // Arrange
        var officeMock = new Mock<IOfficeAppService>();
        officeMock.Setup(l =>
                l.GetStaffListItemsAsync(It.IsAny<Guid?>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ListItems);
        officeMock.Setup(
                l => l.UserIsAssignorAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var staffMock = new Mock<IStaffAppService>();
        staffMock.Setup(l => l.FindCurrentUserAsync())
            .ReturnsAsync(new StaffViewDto { Active = true });
        staffMock.Setup(l => l.HasAppRoleAsync(It.IsAny<string>(), It.IsAny<AppRole>()))
            .ReturnsAsync(false);

        var controller = new OfficeApiController(Mock.Of<IOfficeAppService>(), staffMock.Object);

        // Act
        var response = await controller.GetStaffForAssignmentAsync(Guid.Empty);

        // Assert
        response.Value.Should().BeNull();
    }
}
