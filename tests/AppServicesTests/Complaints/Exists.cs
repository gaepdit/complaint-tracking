using Cts.AppServices.Attachments;
using Cts.AppServices.Complaints;
using Cts.AppServices.Notifications;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities.Complaints;
using Cts.Domain.Entities.Concerns;
using Cts.Domain.Entities.Offices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace AppServicesTests.Complaints;

public class Exists
{
    [Test]
    public async Task WhenItemExists_ReturnsTrue()
    {
        // Arrange
        var repoMock = Substitute.For<IComplaintRepository>();
        repoMock.ExistsAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(true);

        var appService = new ComplaintService(repoMock, Substitute.For<IComplaintManager>(),
            Substitute.For<IConcernRepository>(), Substitute.For<IOfficeRepository>(),
            Substitute.For<IAttachmentService>(), Substitute.For<INotificationService>(), AppServicesTestsSetup.Mapper!,
            Substitute.For<IUserService>(), Substitute.For<IAuthorizationService>(),
            Substitute.For<ILogger<ComplaintService>>());

        // Act
        var result = await appService.ExistsAsync(0);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public async Task WhenNoItemExists_ReturnsFalse()
    {
        // Assert
        var repoMock = Substitute.For<IComplaintRepository>();
        repoMock.ExistsAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(false);

        var appService = new ComplaintService(repoMock, Substitute.For<IComplaintManager>(),
            Substitute.For<IConcernRepository>(), Substitute.For<IOfficeRepository>(),
            Substitute.For<IAttachmentService>(), Substitute.For<INotificationService>(), AppServicesTestsSetup.Mapper!,
            Substitute.For<IUserService>(), Substitute.For<IAuthorizationService>(),
            Substitute.For<ILogger<ComplaintService>>());

        // Act
        var result = await appService.ExistsAsync(0);

        // Arrange
        result.Should().BeFalse();
    }
}
