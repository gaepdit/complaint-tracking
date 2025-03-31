using Cts.Domain.Entities.Attachments;
using Cts.TestData;
using System.Linq.Expressions;

namespace AppServicesTests.Attachments;

public class FindAttachment
{
    [Test]
    public async Task WhenItemsExists_ReturnsViewDtoList()
    {
        // Arrange
        var item = AttachmentData.GetAttachments.First(attachment =>
            attachment is { IsDeleted: false, Complaint: { IsDeleted: false, ComplaintClosed: true } });

        var attachmentRepository = Substitute.For<IAttachmentRepository>();
        attachmentRepository.FindAsync(Arg.Any<Expression<Func<Attachment, bool>>>())
            .Returns(item);

        var appService = AppServiceHelpers.BuildAttachmentService(attachmentRepository: attachmentRepository);

        // Act
        var result = await appService.FindAttachmentAsync(item.Id);

        // Assert
        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenNoItemExists_ReturnsNull()
    {
        // Arrange
        var attachmentRepository = Substitute.For<IAttachmentRepository>();
        attachmentRepository.FindAsync(Arg.Any<Expression<Func<Attachment, bool>>>())
            .Returns((Attachment?)null);

        var appService = AppServiceHelpers.BuildAttachmentService(attachmentRepository: attachmentRepository);

        // Act
        var result = await appService.FindAttachmentAsync(Guid.Empty);

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public async Task WhenItemHasBeenDeleted_ReturnsItem()
    {
        // Arrange
        var item = AttachmentData.GetAttachments.First(attachment => attachment.IsDeleted);

        var attachmentRepository = Substitute.For<IAttachmentRepository>();
        attachmentRepository.FindAsync(Arg.Any<Expression<Func<Attachment, bool>>>())
            .Returns(item);

        var appService = AppServiceHelpers.BuildAttachmentService(attachmentRepository: attachmentRepository);

        // Act
        var result = await appService.FindAttachmentAsync(item.Id);

        // Assert
        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenCorrespondingComplaintIsNotPublic_ReturnsItem()
    {
        // Arrange
        var item = AttachmentData.GetAttachments.First(attachment =>
            attachment is { IsDeleted: false, Complaint.IsDeleted: true });

        var attachmentRepository = Substitute.For<IAttachmentRepository>();
        attachmentRepository.FindAsync(Arg.Any<Expression<Func<Attachment, bool>>>())
            .Returns(item);

        var appService = AppServiceHelpers.BuildAttachmentService(attachmentRepository: attachmentRepository);

        // Act
        var result = await appService.FindAttachmentAsync(item.Id);

        // Assert
        result.Should().BeEquivalentTo(item);
    }
}
