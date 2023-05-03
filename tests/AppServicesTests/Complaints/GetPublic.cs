using Cts.AppServices.Complaints;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities.Attachments;
using Cts.Domain.Entities.ComplaintActions;
using Cts.Domain.Entities.Complaints;
using Cts.Domain.Entities.ComplaintTransitions;
using Cts.Domain.Entities.Concerns;
using Cts.Domain.Entities.Offices;
using Cts.TestData;
using System.Linq.Expressions;

namespace AppServicesTests.Complaints;

public class GetPublic
{
    [Test]
    public async Task WhenItemExists_ReturnsViewDto()
    {
        var complaintActionsList = new List<ComplaintAction>
            { ComplaintActionData.GetComplaintActions.First(e => !e.IsDeleted) };
        var attachmentList = new List<Attachment> { AttachmentData.GetAttachments.First(e => !e.IsDeleted) };
        var item = ComplaintData.GetComplaints.First(e => e is { IsDeleted: false, ComplaintClosed: true });
        item.ComplaintActions = complaintActionsList;
        item.Attachments = attachmentList;
        var repoMock = new Mock<IComplaintRepository>();
        repoMock.Setup(l => l.FindAsync(It.IsAny<Expression<Func<Complaint, bool>>>(), CancellationToken.None))
            .ReturnsAsync(item);
        repoMock.Setup(l =>
                l.GetComplaintActionsListAsync(
                    It.IsAny<Expression<Func<ComplaintAction, bool>>>(), CancellationToken.None))
            .ReturnsAsync(complaintActionsList);
        repoMock.Setup(l =>
                l.GetAttachmentsListAsync(It.IsAny<Expression<Func<Attachment, bool>>>(), CancellationToken.None))
            .ReturnsAsync(attachmentList);
        var appService = new ComplaintService(repoMock.Object, Mock.Of<IComplaintManager>(),
            Mock.Of<IConcernRepository>(), Mock.Of<IOfficeRepository>(), Mock.Of<IComplaintTransitionManager>(),
            AppServicesTestsSetup.Mapper!, Mock.Of<IUserService>());

        var result = await appService.GetPublicAsync(item.Id);

        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenNoItemExists_ReturnsNull()
    {
        var repoMock = new Mock<IComplaintRepository>();
        repoMock.Setup(l => l.FindAsync(It.IsAny<Expression<Func<Complaint, bool>>>(), CancellationToken.None))
            .ReturnsAsync((Complaint?)null);
        var appService = new ComplaintService(repoMock.Object, Mock.Of<IComplaintManager>(),
            Mock.Of<IConcernRepository>(), Mock.Of<IOfficeRepository>(), Mock.Of<IComplaintTransitionManager>(),
            AppServicesTestsSetup.Mapper!, Mock.Of<IUserService>());

        var result = await appService.GetPublicAsync(0);

        result.Should().BeNull();
    }
}
