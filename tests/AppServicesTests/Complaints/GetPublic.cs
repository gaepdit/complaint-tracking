using Cts.AppServices.Complaints;
using Cts.AppServices.UserServices;
using Cts.Domain.Attachments;
using Cts.Domain.ComplaintActions;
using Cts.Domain.Complaints;
using Cts.Domain.Concerns;
using Cts.Domain.Offices;
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
        var appService = new ComplaintAppService(
            repoMock.Object,
            Mock.Of<IConcernRepository>(),
            Mock.Of<IOfficeRepository>(),
            Mock.Of<IComplaintManager>(),
            AppServicesTestsGlobal.Mapper!,
            Mock.Of<IUserService>());

        var result = await appService.GetPublicAsync(item.Id);

        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenNoItemExists_ReturnsNull()
    {
        var repoMock = new Mock<IComplaintRepository>();
        repoMock.Setup(l => l.FindAsync(It.IsAny<Expression<Func<Complaint, bool>>>(), CancellationToken.None))
            .ReturnsAsync((Complaint?)null);
        var appService = new ComplaintAppService(
            repoMock.Object,
            Mock.Of<IConcernRepository>(),
            Mock.Of<IOfficeRepository>(),
            Mock.Of<IComplaintManager>(),
            AppServicesTestsGlobal.Mapper!,
            Mock.Of<IUserService>());

        var result = await appService.GetPublicAsync(0);

        result.Should().BeNull();
    }
}
