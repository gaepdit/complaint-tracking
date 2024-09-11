using Cts.AppServices.Attachments;
using Cts.AppServices.Complaints;
using Cts.AppServices.Notifications;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities.Complaints;
using Cts.Domain.Entities.Concerns;
using Cts.Domain.Entities.Offices;
using Cts.Domain.Identity;
using Cts.TestData.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace AppServicesTests.Complaints;

public class Find
{
    private readonly ApplicationUser _user = new() { Id = Guid.Empty.ToString(), Email = TextData.ValidEmail };

    [Test]
    public async Task WhenItemExists_ReturnsViewDto()
    {
        // Arrange
        var item = new Complaint(1);

        var repoMock = Substitute.For<IComplaintRepository>();
        repoMock.FindIncludeAllAsync(Arg.Any<int>(), Arg.Any<bool>(), Arg.Any<CancellationToken>())
            .Returns(item);

        var userServiceMock = Substitute.For<IUserService>();
        userServiceMock.GetCurrentUserAsync()
            .Returns(_user);

        var authorizationMock = Substitute.For<IAuthorizationService>();
        authorizationMock.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), resource: Arg.Any<object?>(),
                requirements: Arg.Any<IEnumerable<IAuthorizationRequirement>>())
            .Returns(AuthorizationResult.Success());

        var appService = new ComplaintService(repoMock, Substitute.For<IComplaintManager>(),
            Substitute.For<IConcernRepository>(), Substitute.For<IOfficeRepository>(),
            Substitute.For<IAttachmentService>(), Substitute.For<INotificationService>(), AppServicesTestsSetup.Mapper!,
            userServiceMock, authorizationMock, Substitute.For<ILogger<ComplaintService>>());

        // Act
        var result = await appService.FindAsync(item.Id);

        // Assert
        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenNonPublicItemExists_ReturnsViewDto()
    {
        // Arrange
        var item = new Complaint(1);
        item.SetDeleted(null);

        var repoMock = Substitute.For<IComplaintRepository>();
        repoMock.FindIncludeAllAsync(Arg.Any<int>(), Arg.Any<bool>(), Arg.Any<CancellationToken>())
            .Returns(item);

        var userServiceMock = Substitute.For<IUserService>();
        userServiceMock.GetCurrentUserAsync()
            .Returns(_user);

        var authorizationMock = Substitute.For<IAuthorizationService>();
        authorizationMock.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), resource: Arg.Any<object?>(),
                requirements: Arg.Any<IEnumerable<IAuthorizationRequirement>>())
            .Returns(AuthorizationResult.Success());

        var appService = new ComplaintService(repoMock, Substitute.For<IComplaintManager>(),
            Substitute.For<IConcernRepository>(), Substitute.For<IOfficeRepository>(),
            Substitute.For<IAttachmentService>(), Substitute.For<INotificationService>(), AppServicesTestsSetup.Mapper!,
            userServiceMock, authorizationMock, Substitute.For<ILogger<ComplaintService>>());

        // Act
        var result = await appService.FindAsync(item.Id);

        // Assert
        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenNoItemExists_ReturnsNull()
    {
        // Arrange
        var repoMock = Substitute.For<IComplaintRepository>();
        repoMock.FindIncludeAllAsync(Arg.Any<int>(), Arg.Any<bool>(), Arg.Any<CancellationToken>())
            .Returns((Complaint?)null);

        var appService = new ComplaintService(repoMock, Substitute.For<IComplaintManager>(),
            Substitute.For<IConcernRepository>(), Substitute.For<IOfficeRepository>(),
            Substitute.For<IAttachmentService>(), Substitute.For<INotificationService>(), AppServicesTestsSetup.Mapper!,
            Substitute.For<IUserService>(), Substitute.For<IAuthorizationService>(),
            Substitute.For<ILogger<ComplaintService>>());

        // Act
        var result = await appService.FindAsync(0);

        // Assert
        result.Should().BeNull();
    }
}
