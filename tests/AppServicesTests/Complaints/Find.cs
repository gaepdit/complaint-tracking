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

public class Find
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
        var repoMock = Substitute.For<IComplaintRepository>();
        repoMock.FindAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(item);
        repoMock.GetComplaintActionsListAsync(
                Arg.Any<Expression<Func<ComplaintAction, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(complaintActionsList);
        repoMock.GetAttachmentsListAsync(Arg.Any<Expression<Func<Attachment, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(attachmentList);
        var appService = new ComplaintService(repoMock, Substitute.For<IComplaintManager>(),
            Substitute.For<IConcernRepository>(), Substitute.For<IOfficeRepository>(),
            Substitute.For<IComplaintTransitionManager>(),
            AppServicesTestsSetup.Mapper!, Substitute.For<IUserService>());

        var result = await appService.FindAsync(item.Id);

        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenNonPublicItemExists_ReturnsViewDto()
    {
        var complaintActionsList = new List<ComplaintAction>
            { ComplaintActionData.GetComplaintActions.First(e => !e.IsDeleted) };
        var attachmentList = new List<Attachment> { AttachmentData.GetAttachments.First(e => e.IsDeleted) };
        var item = ComplaintData.GetComplaints.First(e => e.IsDeleted);
        item.ComplaintActions = complaintActionsList;
        item.Attachments = attachmentList;
        var repoMock = Substitute.For<IComplaintRepository>();
        repoMock.FindAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(item);
        repoMock.GetComplaintActionsListAsync(
                Arg.Any<Expression<Func<ComplaintAction, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(complaintActionsList);
        repoMock.GetAttachmentsListAsync(
                Arg.Any<Expression<Func<Attachment, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(attachmentList);
        var appService = new ComplaintService(repoMock, Substitute.For<IComplaintManager>(),
            Substitute.For<IConcernRepository>(), Substitute.For<IOfficeRepository>(),
            Substitute.For<IComplaintTransitionManager>(),
            AppServicesTestsSetup.Mapper!, Substitute.For<IUserService>());

        var result = await appService.FindAsync(item.Id);

        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenNoItemExists_ReturnsNull()
    {
        var repoMock = Substitute.For<IComplaintRepository>();
        repoMock.FindAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns((Complaint?)null);
        var appService = new ComplaintService(repoMock, Substitute.For<IComplaintManager>(),
            Substitute.For<IConcernRepository>(), Substitute.For<IOfficeRepository>(),
            Substitute.For<IComplaintTransitionManager>(),
            AppServicesTestsSetup.Mapper!, Substitute.For<IUserService>());

        var result = await appService.FindAsync(0);

        result.Should().BeNull();
    }
}
