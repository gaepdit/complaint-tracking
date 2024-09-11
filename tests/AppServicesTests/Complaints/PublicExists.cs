using Cts.AppServices.Attachments;
using Cts.AppServices.Complaints;
using Cts.AppServices.Notifications;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities.Complaints;
using Cts.Domain.Entities.Concerns;
using Cts.Domain.Entities.Offices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace AppServicesTests.Complaints;

public class PublicExists
{
    [Test]
    public async Task WhenItemExists_ReturnsTrue()
    {
        // Arrange
        var repoMock = Substitute.For<IComplaintRepository>();
        repoMock.ExistsAsync(Arg.Any<Expression<Func<Complaint, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(true);

        var appService = new ComplaintService(repoMock, Substitute.For<IComplaintManager>(),
            Substitute.For<IConcernRepository>(), Substitute.For<IOfficeRepository>(),
            Substitute.For<IAttachmentService>(), Substitute.For<INotificationService>(), AppServicesTestsSetup.Mapper!,
            Substitute.For<IUserService>(), Substitute.For<IAuthorizationService>(),
            Substitute.For<ILogger<ComplaintService>>());

        // Act
        var result = await appService.PublicExistsAsync(0);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public async Task WhenNoItemExists_ReturnsFalse()
    {
        // Arrange
        var repoMock = Substitute.For<IComplaintRepository>();
        repoMock.ExistsAsync(Arg.Any<Expression<Func<Complaint, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(false);

        var appService = new ComplaintService(repoMock, Substitute.For<IComplaintManager>(),
            Substitute.For<IConcernRepository>(), Substitute.For<IOfficeRepository>(),
            Substitute.For<IAttachmentService>(), Substitute.For<INotificationService>(), AppServicesTestsSetup.Mapper!,
            Substitute.For<IUserService>(), Substitute.For<IAuthorizationService>(),
            Substitute.For<ILogger<ComplaintService>>());

        // Act
        var result = await appService.PublicExistsAsync(0);

        // Assert
        result.Should().BeFalse();
    }
}
