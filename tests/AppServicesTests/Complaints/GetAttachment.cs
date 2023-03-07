using Cts.AppServices.Complaints;
using Cts.AppServices.UserServices;
using Cts.Domain.Attachments;
using Cts.Domain.Complaints;
using Cts.TestData;
using System.Linq.Expressions;

namespace AppServicesTests.Complaints;

public class GetAttachment
{
    [Test]
    public async Task WhenItemsExists_ReturnsViewDtoList()
    {
        var item = AttachmentData.GetAttachments.First(e => 
            !e.IsDeleted && !e.Complaint.IsDeleted && e.Complaint.ComplaintClosed);
        var repoMock = new Mock<IComplaintRepository>();
        repoMock.Setup(l => l.FindAttachmentAsync(It.IsAny<Guid>(), CancellationToken.None))
            .ReturnsAsync(item);
        var appService = new ComplaintAppService(repoMock.Object, Mock.Of<IComplaintManager>(),
            AppServicesTestsGlobal.Mapper!, Mock.Of<IUserService>());

        var result = await appService.GetAttachmentAsync(item.Id);

        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenNoItemExists_ReturnsNull()
    {
        var repoMock = new Mock<IComplaintRepository>();
        repoMock.Setup(l => l.FindAttachmentAsync(It.IsAny<Guid>(), CancellationToken.None))
            .ReturnsAsync((Attachment?)null);
        var appService = new ComplaintAppService(repoMock.Object, Mock.Of<IComplaintManager>(),
            AppServicesTestsGlobal.Mapper!, Mock.Of<IUserService>());

        var result = await appService.GetAttachmentAsync(Guid.Empty);

        result.Should().BeNull();
    }

    [Test]
    public async Task WhenItemHasBeenDeleted_ReturnsItem()
    {
        var item = AttachmentData.GetAttachments.First(e => e.IsDeleted);
        var repoMock = new Mock<IComplaintRepository>();
        repoMock.Setup(l => l.FindAttachmentAsync(It.IsAny<Guid>(), CancellationToken.None))
            .ReturnsAsync(item);
        var appService = new ComplaintAppService(repoMock.Object, Mock.Of<IComplaintManager>(),
            AppServicesTestsGlobal.Mapper!, Mock.Of<IUserService>());

        var result = await appService.GetAttachmentAsync(item.Id);

        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenCorrespondingComplaintIsNotPublic_ReturnsItem()
    {
        var item = AttachmentData.GetAttachments.First(e => !e.IsDeleted && e.Complaint.IsDeleted);

        var repoMock = new Mock<IComplaintRepository>();
        repoMock.Setup(l => l.FindAttachmentAsync(It.IsAny<Guid>(), CancellationToken.None))
            .ReturnsAsync(item);
        var appService = new ComplaintAppService(repoMock.Object, Mock.Of<IComplaintManager>(),
            AppServicesTestsGlobal.Mapper!, Mock.Of<IUserService>());

        var result = await appService.GetAttachmentAsync(item.Id);

        result.Should().BeEquivalentTo(item);
    }
}
