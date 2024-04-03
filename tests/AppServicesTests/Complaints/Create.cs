using Cts.AppServices.Attachments;
using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.CommandDto;
using Cts.AppServices.Email;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities.Complaints;
using Cts.Domain.Entities.Concerns;
using Cts.Domain.Entities.Offices;
using Cts.Domain.Identity;
using Cts.TestData.Constants;
using GaEpd.EmailService;
using Microsoft.AspNetCore.Authorization;

namespace AppServicesTests.Complaints;

public class Create
{
    [Test]
    public async Task OnSuccessfulInsert_ReturnsSuccessfully()
    {
        // Arrange
        const int id = 99;
        var complaintManagerMock = Substitute.For<IComplaintManager>();
        complaintManagerMock.Create(Arg.Any<ApplicationUser?>())
            .Returns(new Complaint(id) { CurrentOffice = new Office() });

        var userServiceMock = Substitute.For<IUserService>();
        userServiceMock.GetCurrentUserAsync()
            .Returns(new ApplicationUser { Id = Guid.Empty.ToString() });
        userServiceMock.GetUserAsync(Arg.Any<string>())
            .Returns(new ApplicationUser { Id = Guid.Empty.ToString() });

        var officeRepoMock = Substitute.For<IOfficeRepository>();
        officeRepoMock.GetAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(new Office(Guid.NewGuid(), TextData.ValidName));

        var notificationMock = Substitute.For<INotificationService>();
        notificationMock.SendNotificationAsync(Arg.Any<EmailTemplate>(), Arg.Any<string>(), Arg.Any<Complaint>(),
                Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(OperationResult.SuccessResult());

        var appService = new ComplaintService(Substitute.For<IComplaintRepository>(), complaintManagerMock,
            Substitute.For<IConcernRepository>(), officeRepoMock, Substitute.For<IAttachmentService>(),
            notificationMock, AppServicesTestsSetup.Mapper!, userServiceMock, Substitute.For<IAuthorizationService>());

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
            .Returns(new ApplicationUser { Id = Guid.Empty.ToString() });

        var office = new Office(Guid.NewGuid(), TextData.ValidName);
        var officeRepoMock = Substitute.For<IOfficeRepository>();
        officeRepoMock.GetAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(office);

        var notificationMock = Substitute.For<INotificationService>();
        notificationMock.SendNotificationAsync(Arg.Any<EmailTemplate>(), Arg.Any<string>(), Arg.Any<Complaint>(),
                Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(OperationResult.SuccessResult());

        var appService = new ComplaintService(Substitute.For<IComplaintRepository>(), complaintManagerMock,
            Substitute.For<IConcernRepository>(), officeRepoMock, Substitute.For<IAttachmentService>(),
            notificationMock, AppServicesTestsSetup.Mapper!, userServiceMock, Substitute.For<IAuthorizationService>());

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
