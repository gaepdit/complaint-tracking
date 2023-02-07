using Cts.AppServices.Complaints;
using Cts.Domain.Attachments;
using Cts.Domain.Complaints;
using Cts.TestData;
using System.Linq.Expressions;

namespace AppServicesTests.Complaints;

public class GetPublicAttachment
{
    [Test]
    public async Task WhenItemsExists_ReturnsViewDtoList()
    {
        var item = AttachmentData.GetAttachments.First(e => !e.IsDeleted);
        var complaint = ComplaintData.GetComplaints.First(e => e is { IsDeleted: false, ComplaintClosed: true });
        var repoMock = new Mock<IComplaintRepository>();
        repoMock.Setup(l => l.FindAttachmentAsync(item.Id, CancellationToken.None))
            .ReturnsAsync(item);
        repoMock.Setup(l => l.FindAsync(It.IsAny<Expression<Func<Complaint, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(complaint);
        var appService = new ComplaintAppService(repoMock.Object, AppServicesTestsGlobal.Mapper!);

        var result = await appService.GetPublicAttachmentAsync(item.Id);

        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenNoItemExists_ReturnsNull()
    {
        var repoMock = new Mock<IComplaintRepository>();
        repoMock.Setup(l => l.FindAttachmentAsync(It.IsAny<Guid>(), CancellationToken.None))
            .ReturnsAsync((Attachment?)null);
        var appService = new ComplaintAppService(repoMock.Object, AppServicesTestsGlobal.Mapper!);

        var result = await appService.GetPublicAttachmentAsync(Guid.Empty);

        result.Should().BeNull();
    }

    [Test]
    public async Task WhenItemHasBeenDeleted_ReturnsNull()
    {
        var item = AttachmentData.GetAttachments.First(e => e.IsDeleted);
        var repoMock = new Mock<IComplaintRepository>();
        repoMock.Setup(l => l.FindAttachmentAsync(It.IsAny<Guid>(), CancellationToken.None))
            .ReturnsAsync(item);
        var appService = new ComplaintAppService(repoMock.Object, AppServicesTestsGlobal.Mapper!);

        var result = await appService.GetPublicAttachmentAsync(item.Id);

        result.Should().BeNull();
    }

    [Test]
    public async Task WhenNoCorrespondingComplaintExists_ReturnsNull()
    {
        var item = AttachmentData.GetAttachments.First(e => !e.IsDeleted);
        var repoMock = new Mock<IComplaintRepository>();
        repoMock.Setup(l => l.FindAttachmentAsync(item.Id, CancellationToken.None))
            .ReturnsAsync(item);
        repoMock.Setup(l => l.FindAsync(It.IsAny<int>(), CancellationToken.None))
            .ReturnsAsync((Complaint?)null);
        var appService = new ComplaintAppService(repoMock.Object, AppServicesTestsGlobal.Mapper!);

        var result = await appService.GetPublicAttachmentAsync(item.Id);

        result.Should().BeNull();
    }
}
