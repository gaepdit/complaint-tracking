using Cts.AppServices.Attachments;
using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.CommandDto;
using Cts.AppServices.Notifications;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities.Complaints;
using Cts.Domain.Entities.Concerns;
using Cts.Domain.Entities.Offices;
using Cts.Domain.Identity;
using Cts.TestData.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace AppServicesTests.Complaints;

public class Create
{
    private readonly ApplicationUser _user = new() { Id = Guid.Empty.ToString(), Email = TextData.ValidEmail };

    [Test]
    public async Task OnSuccessfulInsert_ReturnsSuccessfully()
    {
        // Arrange
        const int id = 99;
        var complaintManagerMock = Substitute.For<IComplaintManager>();
        complaintManagerMock.Create(Arg.Any<ApplicationUser?>())
            .Returns(new Complaint(id) { CurrentOffice = new Office(), CurrentOwner = _user });

        var userServiceMock = Substitute.For<IUserService>();
        userServiceMock.GetCurrentUserAsync()
            .Returns(_user);
        userServiceMock.GetUserAsync(Arg.Any<string>())
            .Returns(_user);
        userServiceMock.FindUserAsync(Arg.Any<string>())
            .Returns(_user);

        var officeRepoMock = Substitute.For<IOfficeRepository>();
        officeRepoMock.GetAsync(Arg.Any<Guid>())
            .Returns(new Office(Guid.NewGuid(), TextData.ValidName));

        var notificationMock = Substitute.For<INotificationService>();
        notificationMock.SendNotificationAsync(Arg.Any<Template>(), Arg.Any<string>(), Arg.Any<Complaint>(),
                Arg.Any<string>(), Arg.Any<string>())
            .Returns(NotificationResult.SuccessResult());

        var appService = new ComplaintService(Substitute.For<IComplaintRepository>(), complaintManagerMock,
            Substitute.For<IConcernRepository>(), officeRepoMock, Substitute.For<IAttachmentService>(),
            notificationMock, AppServicesTestsSetup.Mapper!, userServiceMock, Substitute.For<IAuthorizationService>(),
            Substitute.For<ILogger<ComplaintService>>());

        var item = new ComplaintCreateDto { OfficeId = Guid.Empty };

        // Act
        var result = await appService.CreateAsync(item, AppServiceHelpers.AttachmentServiceConfig, null,
            CancellationToken.None);

        // Assert
        using var scope = new AssertionScope();
        result.HasWarnings.Should().BeFalse();
        result.ComplaintId.Should().Be(id);
    }

    [Test]
    public async Task CreateComplaint_CalculatesCorrectDateTimeOffset()
    {
        // Arrange
        var complaintManagerMock = Substitute.For<IComplaintManager>();
        complaintManagerMock.Create(null)
            .Returns(new Complaint(0));

        var userServiceMock = Substitute.For<IUserService>();
        userServiceMock.GetCurrentUserAsync()
            .Returns(_user);

        var office = new Office(Guid.NewGuid(), TextData.ValidName);
        var officeRepoMock = Substitute.For<IOfficeRepository>();
        officeRepoMock.GetAsync(Arg.Any<Guid>())
            .Returns(office);

        var notificationMock = Substitute.For<INotificationService>();
        notificationMock.SendNotificationAsync(Arg.Any<Template>(), Arg.Any<string>(), Arg.Any<Complaint>(),
                Arg.Any<string>(), Arg.Any<string>())
            .Returns(NotificationResult.SuccessResult());

        var appService = new ComplaintService(Substitute.For<IComplaintRepository>(), complaintManagerMock,
            Substitute.For<IConcernRepository>(), officeRepoMock, Substitute.For<IAttachmentService>(),
            notificationMock, AppServicesTestsSetup.Mapper!, userServiceMock, Substitute.For<IAuthorizationService>(),
            Substitute.For<ILogger<ComplaintService>>());

        var item = new ComplaintCreateDto
        {
            ReceivedDate = new DateOnly(2000, 1, 1),
            ReceivedTime = new TimeOnly(1, 15, 0),
            OfficeId = office.Id,
        };

        var expected = new DateTime(2000, 1, 1, 1, 15, 0, DateTimeKind.Local);

        // Act
        var result = await appService.CreateComplaintFromDtoAsync(item, null, CancellationToken.None);

        // Assert
        result.ReceivedDate.Should().Be(expected);
    }
}
