using Cts.AppServices.Complaints;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities.Attachments;
using Cts.Domain.Entities.Complaints;
using Cts.Domain.Entities.ComplaintTransitions;
using Cts.Domain.Entities.Concerns;
using Cts.Domain.Entities.Offices;
using Cts.TestData;
using System.Linq.Expressions;

namespace AppServicesTests.Complaints;

public class FindAttachment
{
    [Test]
    public async Task WhenItemsExists_ReturnsViewDtoList()
    {
        var item = AttachmentData.GetAttachments.First(e =>
            e is { IsDeleted: false, Complaint: { IsDeleted: false, ComplaintClosed: true } });
        var repoMock = Substitute.For<IComplaintRepository>();
        repoMock.FindAttachmentAsync(Arg.Any<Expression<Func<Attachment, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(item);
        var appService = new ComplaintService(repoMock, Substitute.For<IComplaintManager>(),
            Substitute.For<IConcernRepository>(), Substitute.For<IOfficeRepository>(),
            Substitute.For<IComplaintTransitionManager>(),
            AppServicesTestsSetup.Mapper!, Substitute.For<IUserService>());

        var result = await appService.FindAttachmentAsync(item.Id);

        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenNoItemExists_ReturnsNull()
    {
        var repoMock = Substitute.For<IComplaintRepository>();
        repoMock.FindAttachmentAsync(Arg.Any<Expression<Func<Attachment, bool>>>(), Arg.Any<CancellationToken>())
            .Returns((Attachment?)null);
        var appService = new ComplaintService(repoMock, Substitute.For<IComplaintManager>(),
            Substitute.For<IConcernRepository>(), Substitute.For<IOfficeRepository>(),
            Substitute.For<IComplaintTransitionManager>(),
            AppServicesTestsSetup.Mapper!, Substitute.For<IUserService>());

        var result = await appService.FindAttachmentAsync(Guid.Empty);

        result.Should().BeNull();
    }

    [Test]
    public async Task WhenItemHasBeenDeleted_ReturnsItem()
    {
        var item = AttachmentData.GetAttachments.First(e => e.IsDeleted);
        var repoMock = Substitute.For<IComplaintRepository>();
        repoMock.FindAttachmentAsync(Arg.Any<Expression<Func<Attachment, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(item);
        var appService = new ComplaintService(repoMock, Substitute.For<IComplaintManager>(),
            Substitute.For<IConcernRepository>(), Substitute.For<IOfficeRepository>(),
            Substitute.For<IComplaintTransitionManager>(),
            AppServicesTestsSetup.Mapper!, Substitute.For<IUserService>());

        var result = await appService.FindAttachmentAsync(item.Id);

        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenCorrespondingComplaintIsNotPublic_ReturnsItem()
    {
        var item = AttachmentData.GetAttachments.First(e => e is { IsDeleted: false, Complaint.IsDeleted: true });

        var repoMock = Substitute.For<IComplaintRepository>();
        repoMock.FindAttachmentAsync(Arg.Any<Expression<Func<Attachment, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(item);
        var appService = new ComplaintService(repoMock, Substitute.For<IComplaintManager>(),
            Substitute.For<IConcernRepository>(), Substitute.For<IOfficeRepository>(),
            Substitute.For<IComplaintTransitionManager>(),
            AppServicesTestsSetup.Mapper!, Substitute.For<IUserService>());

        var result = await appService.FindAttachmentAsync(item.Id);

        result.Should().BeEquivalentTo(item);
    }
}
