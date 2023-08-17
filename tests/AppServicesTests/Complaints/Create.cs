using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.Dto;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities.Complaints;
using Cts.Domain.Entities.ComplaintTransitions;
using Cts.Domain.Entities.Concerns;
using Cts.Domain.Entities.Offices;
using Cts.Domain.Identity;
using Cts.TestData.Constants;

namespace AppServicesTests.Complaints;

public class Create
{
    [Test]
    public async Task OnSuccessfulInsert_ReturnsId()
    {
        // Arrange
        var complaintManagerMock = Substitute.For<IComplaintManager>();
        complaintManagerMock.CreateNewComplaintAsync()
            .Returns(new Complaint(0));

        var userServiceMock = Substitute.For<IUserService>();
        userServiceMock.GetCurrentUserAsync()
            .Returns(new ApplicationUser { Id = Guid.Empty.ToString() });

        var officeRepoMock = Substitute.For<IOfficeRepository>();
        officeRepoMock.GetAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(new Office(Guid.NewGuid(), TestConstants.ValidName));

        var appService = new ComplaintService(Substitute.For<IComplaintRepository>(), complaintManagerMock,
            Substitute.For<IConcernRepository>(), officeRepoMock, Substitute.For<IComplaintTransitionManager>(),
            AppServicesTestsSetup.Mapper!, userServiceMock);

        var item = new ComplaintCreateDto { CurrentOfficeId = Guid.Empty };

        // Act
        var result = await appService.CreateAsync(item);

        // Assert
        result.Should().Be(0);
    }

    [Test]
    public async Task CreateComplaint_CalculatesCorrectDateTimeOffset()
    {
        // Arrange
        var complaintManagerMock = Substitute.For<IComplaintManager>();
        complaintManagerMock.CreateNewComplaintAsync()
            .Returns(new Complaint(0));

        var userServiceMock = Substitute.For<IUserService>();
        userServiceMock.GetCurrentUserAsync()
            .Returns(new ApplicationUser { Id = Guid.Empty.ToString() });

        var office = new Office(Guid.NewGuid(), TestConstants.ValidName);
        var officeRepoMock = Substitute.For<IOfficeRepository>();
        officeRepoMock.GetAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(office);

        var appService = new ComplaintService(Substitute.For<IComplaintRepository>(), complaintManagerMock,
            Substitute.For<IConcernRepository>(), officeRepoMock, Substitute.For<IComplaintTransitionManager>(),
            AppServicesTestsSetup.Mapper!, userServiceMock);

        var item = new ComplaintCreateDto
        {
            ReceivedDate = new DateTime(2000, 1, 1),
            ReceivedTime = new DateTime(2020, 2, 2, 1, 15, 0),
            CurrentOfficeId = office.Id,
        };

        item.ReceivedDate = DateTime.SpecifyKind(item.ReceivedDate, DateTimeKind.Local);

        var correct = new DateTime(2000, 1, 1, 1, 15, 0);
        var expected = DateTime.SpecifyKind(correct, DateTimeKind.Local);

        // Act
        var result = await appService.CreateComplaintFromDtoAsync(item, null, CancellationToken.None);

        // Assert
        result.ReceivedDate.Should().Be(expected);
    }
}
