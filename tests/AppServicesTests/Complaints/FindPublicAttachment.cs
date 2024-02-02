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

public class FindPublicAttachment
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

        var result = await appService.FindPublicAttachmentAsync(item.Id);

        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenNoPublicItemExists_ReturnsNull()
    {
        var repoMock = Substitute.For<IComplaintRepository>();
        repoMock.FindAttachmentAsync(Arg.Any<Expression<Func<Attachment, bool>>>(), Arg.Any<CancellationToken>())
            .Returns((Attachment?)null);
        var appService = new ComplaintService(repoMock, Substitute.For<IComplaintManager>(),
            Substitute.For<IConcernRepository>(), Substitute.For<IOfficeRepository>(),
            Substitute.For<IComplaintTransitionManager>(),
            AppServicesTestsSetup.Mapper!, Substitute.For<IUserService>());

        var result = await appService.FindPublicAttachmentAsync(Guid.Empty);

        result.Should().BeNull();
    }
}
