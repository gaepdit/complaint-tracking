using Cts.AppServices.Complaints;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities.Attachments;
using Cts.Domain.Entities.Complaints;
using Cts.Domain.Entities.ComplaintTransitions;
using Cts.Domain.Entities.Concerns;
using Cts.Domain.Entities.Offices;
using Cts.TestData;

namespace AppServicesTests.Complaints;

public class GetPublicAttachment
{
    [Test]
    public async Task WhenItemsExists_ReturnsViewDtoList()
    {
        var item = AttachmentData.GetAttachments.First(e =>
            e is { IsDeleted: false, Complaint: { IsDeleted: false, ComplaintClosed: true } });
        var repoMock = new Mock<IComplaintRepository>();
        repoMock.Setup(l => l.FindAttachmentAsync(It.IsAny<Guid>(), CancellationToken.None))
            .ReturnsAsync(item);
        var appService = new ComplaintAppService(repoMock.Object, Mock.Of<IComplaintManager>(),
            Mock.Of<IConcernRepository>(), Mock.Of<IOfficeRepository>(), Mock.Of<IComplaintTransitionRepository>(),
            Mock.Of<IComplaintTransitionManager>(), AppServicesTestsGlobal.Mapper!, Mock.Of<IUserService>());

        var result = await appService.GetPublicAttachmentAsync(item.Id);

        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenNoItemExists_ReturnsNull()
    {
        var repoMock = new Mock<IComplaintRepository>();
        repoMock.Setup(l => l.FindAttachmentAsync(It.IsAny<Guid>(), CancellationToken.None))
            .ReturnsAsync((Attachment?)null);
        var appService = new ComplaintAppService(repoMock.Object, Mock.Of<IComplaintManager>(),
            Mock.Of<IConcernRepository>(), Mock.Of<IOfficeRepository>(), Mock.Of<IComplaintTransitionRepository>(),
            Mock.Of<IComplaintTransitionManager>(), AppServicesTestsGlobal.Mapper!, Mock.Of<IUserService>());

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
        var appService = new ComplaintAppService(repoMock.Object, Mock.Of<IComplaintManager>(),
            Mock.Of<IConcernRepository>(), Mock.Of<IOfficeRepository>(), Mock.Of<IComplaintTransitionRepository>(),
            Mock.Of<IComplaintTransitionManager>(), AppServicesTestsGlobal.Mapper!, Mock.Of<IUserService>());

        var result = await appService.GetPublicAttachmentAsync(item.Id);

        result.Should().BeNull();
    }

    [Test]
    public async Task WhenCorrespondingComplaintIsNotPublic_ReturnsNull()
    {
        var item = AttachmentData.GetAttachments.First(e => e is { IsDeleted: false, Complaint.IsDeleted: true });

        var repoMock = new Mock<IComplaintRepository>();
        repoMock.Setup(l => l.FindAttachmentAsync(It.IsAny<Guid>(), CancellationToken.None))
            .ReturnsAsync(item);
        var appService = new ComplaintAppService(repoMock.Object, Mock.Of<IComplaintManager>(),
            Mock.Of<IConcernRepository>(), Mock.Of<IOfficeRepository>(), Mock.Of<IComplaintTransitionRepository>(),
            Mock.Of<IComplaintTransitionManager>(), AppServicesTestsGlobal.Mapper!, Mock.Of<IUserService>());

        var result = await appService.GetPublicAttachmentAsync(item.Id);

        result.Should().BeNull();
    }
}
