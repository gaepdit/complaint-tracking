using Cts.AppServices.Complaints;
using Cts.Domain.Attachments;
using Cts.Domain.Complaints;
using Cts.TestData;
using System.Linq.Expressions;

namespace AppServicesTests.Complaints;

public class GetPublicView
{
    [Test]
    public async Task WhenItemExists_ReturnsViewDto()
    {
        var attachmentList = new List<Attachment> { AttachmentData.GetAttachments.First(e => !e.IsDeleted) };
        var item = ComplaintData.GetComplaints.First(e => e is { IsDeleted: false, ComplaintClosed: true });
        item.Attachments = attachmentList;
        var repoMock = new Mock<IComplaintRepository>();
        repoMock.Setup(l => l.FindAsync(It.IsAny<Expression<Func<Complaint, bool>>>(), CancellationToken.None))
            .ReturnsAsync(item);
        repoMock.Setup(l =>
                l.GetAttachmentsListAsync(It.IsAny<Expression<Func<Attachment, bool>>>(), CancellationToken.None))
            .ReturnsAsync(attachmentList);
        var appService = new ComplaintAppService(repoMock.Object, AppServicesTestsGlobal.Mapper!);

        var result = await appService.GetPublicViewAsync(item.Id);

        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenNoItemExists_ReturnsNull()
    {
        var repoMock = new Mock<IComplaintRepository>();
        repoMock.Setup(l => l.FindAsync(It.IsAny<Expression<Func<Complaint, bool>>>(), CancellationToken.None))
            .ReturnsAsync((Complaint?)null);
        var appService = new ComplaintAppService(repoMock.Object, AppServicesTestsGlobal.Mapper!);

        var result = await appService.GetPublicViewAsync(0);

        result.Should().BeNull();
    }
}
