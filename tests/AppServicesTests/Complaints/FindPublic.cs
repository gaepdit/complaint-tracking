using Cts.AppServices.Complaints;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities.Complaints;
using Cts.Domain.Entities.ComplaintTransitions;
using Cts.Domain.Entities.Concerns;
using Cts.Domain.Entities.Offices;
using Cts.TestData;
using System.Linq.Expressions;

namespace AppServicesTests.Complaints;

public class FindPublic
{
    [Test]
    public async Task WhenItemExists_ReturnsViewDto()
    {
        // Arrange
        var item = ComplaintData.GetComplaints.First(e => e is { IsDeleted: false, ComplaintClosed: true });
        item.ComplaintActions = [ComplaintActionData.GetComplaintActions.First(e => !e.IsDeleted)];
        item.Attachments = [AttachmentData.GetAttachments.First(e => !e.IsDeleted)];

        var repoMock = Substitute.For<IComplaintRepository>();
        repoMock.FindIncludeAllAsync(Arg.Any<Expression<Func<Complaint, bool>>>(), Arg.Any<bool>(),
                Arg.Any<CancellationToken>())
            .Returns(item);

        var appService = new ComplaintService(repoMock, Substitute.For<IComplaintManager>(),
            Substitute.For<IConcernRepository>(), Substitute.For<IOfficeRepository>(),
            Substitute.For<IComplaintTransitionManager>(),
            AppServicesTestsSetup.Mapper!, Substitute.For<IUserService>());

        // Act
        var result = await appService.FindPublicAsync(item.Id);

        // Assert
        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenNoItemExists_ReturnsNull()
    {
        var repoMock = Substitute.For<IComplaintRepository>();
        repoMock.FindAsync(Arg.Any<Expression<Func<Complaint, bool>>>(), Arg.Any<CancellationToken>())
            .Returns((Complaint?)null);
        var appService = new ComplaintService(repoMock, Substitute.For<IComplaintManager>(),
            Substitute.For<IConcernRepository>(), Substitute.For<IOfficeRepository>(),
            Substitute.For<IComplaintTransitionManager>(),
            AppServicesTestsSetup.Mapper!, Substitute.For<IUserService>());

        var result = await appService.FindPublicAsync(0);

        result.Should().BeNull();
    }
}
