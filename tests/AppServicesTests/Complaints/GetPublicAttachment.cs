using Cts.AppServices.Complaints;
using Cts.AppServices.UserServices;
using Cts.Domain.Attachments;
using Cts.Domain.Complaints;
using Cts.Domain.Concerns;
using Cts.Domain.Offices;
using Cts.TestData;

namespace AppServicesTests.Complaints;

public class GetPublicAttachment
{
    [Test]
    public async Task WhenItemsExists_ReturnsViewDtoList()
    {
        var item = AttachmentData.GetAttachments.First(e =>
            !e.IsDeleted && !e.Complaint.IsDeleted && e.Complaint.ComplaintClosed);
        var repoMock = new Mock<IComplaintRepository>();
        repoMock.Setup(l => l.FindAttachmentAsync(It.IsAny<Guid>(), CancellationToken.None))
            .ReturnsAsync(item);
        var appService = new ComplaintAppService(
            repoMock.Object,
            Mock.Of<IConcernRepository>(),
            Mock.Of<IOfficeRepository>(),
            Mock.Of<IComplaintManager>(),
            AppServicesTestsGlobal.Mapper!,
            Mock.Of<IUserService>());

        var result = await appService.GetPublicAttachmentAsync(item.Id);

        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenNoItemExists_ReturnsNull()
    {
        var repoMock = new Mock<IComplaintRepository>();
        repoMock.Setup(l => l.FindAttachmentAsync(It.IsAny<Guid>(), CancellationToken.None))
            .ReturnsAsync((Attachment?)null);
        var appService = new ComplaintAppService(
            repoMock.Object,
            Mock.Of<IConcernRepository>(),
            Mock.Of<IOfficeRepository>(),
            Mock.Of<IComplaintManager>(),
            AppServicesTestsGlobal.Mapper!,
            Mock.Of<IUserService>());

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
        var appService = new ComplaintAppService(
            repoMock.Object,
            Mock.Of<IConcernRepository>(),
            Mock.Of<IOfficeRepository>(),
            Mock.Of<IComplaintManager>(),
            AppServicesTestsGlobal.Mapper!,
            Mock.Of<IUserService>());

        var result = await appService.GetPublicAttachmentAsync(item.Id);

        result.Should().BeNull();
    }

    [Test]
    public async Task WhenCorrespondingComplaintIsNotPublic_ReturnsNull()
    {
        var item = AttachmentData.GetAttachments.First(e => !e.IsDeleted && e.Complaint.IsDeleted);

        var repoMock = new Mock<IComplaintRepository>();
        repoMock.Setup(l => l.FindAttachmentAsync(It.IsAny<Guid>(), CancellationToken.None))
            .ReturnsAsync(item);
        var appService = new ComplaintAppService(
            repoMock.Object,
            Mock.Of<IConcernRepository>(),
            Mock.Of<IOfficeRepository>(),
            Mock.Of<IComplaintManager>(),
            AppServicesTestsGlobal.Mapper!,
            Mock.Of<IUserService>());

        var result = await appService.GetPublicAttachmentAsync(item.Id);

        result.Should().BeNull();
    }
}
