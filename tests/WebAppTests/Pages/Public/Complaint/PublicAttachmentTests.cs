using Cts.AppServices.Attachments;
using Cts.AppServices.Complaints;
using Cts.WebApp.Pages.Public.Complaints;

namespace WebAppTests.Pages.Public.Complaint;

public class PublicAttachmentTests
{
    [Test]
    public async Task OnGet_AttachmentExistsAndFileNameMatches_ReturnsFile()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var fileBytes = new byte[] { 0x0 };
        var attachment = new AttachmentViewDto
        {
            Id = guid,
            FileName = ".pdf",
            FileExtension = ".pdf",
        };

        var complaintService = Substitute.For<IComplaintService>();
        complaintService.FindPublicAttachmentAsync(guid).Returns(attachment);

        var fileService = Substitute.For<IAttachmentFileService>();
        fileService.GetAttachmentFileAsync(Arg.Any<string>(), Arg.Any<bool>()).Returns(fileBytes);

        var pageModel = new AttachmentModel(complaintService, fileService);

        // Act
        var result = await pageModel.OnGetAsync(guid, attachment.FileName);

        // Assert
        using var scope = new AssertionScope();
        result.Should().BeOfType<FileContentResult>();
        ((FileContentResult)result).FileContents.Should().BeEquivalentTo(fileBytes);
        ((FileContentResult)result).ContentType.Should().Be("application/pdf");
    }

    [Test]
    public async Task OnGet_AttachmentDoesNotExist_ReturnsNotfound()
    {
        // Arrange
        var guid = Guid.NewGuid();

        var complaintService = Substitute.For<IComplaintService>();
        complaintService.FindPublicAttachmentAsync(guid).Returns((AttachmentViewDto?)null);

        var pageModel = new AttachmentModel(complaintService, Substitute.For<IAttachmentFileService>());

        // Act
        var result = await pageModel.OnGetAsync(guid, TextData.ShortName);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Test]
    public async Task OnGet_FileNameDoesNotMatch_RedirectsToValidFileName()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var attachment = new AttachmentViewDto
        {
            Id = guid,
            FileName = ".pdf",
            FileExtension = ".pdf",
        };

        var complaintService = Substitute.For<IComplaintService>();
        complaintService.FindPublicAttachmentAsync(guid).Returns(attachment);

        var pageModel = new AttachmentModel(complaintService, Substitute.For<IAttachmentFileService>());

        // Act
        var result = await pageModel.OnGetAsync(guid, TextData.NonExistentName);

        // Act
        using var scope = new AssertionScope();
        result.Should().BeOfType<RedirectToPageResult>();
        ((RedirectToPageResult)result).PageName.Should().Be("Attachment");
    }

    [Test]
    public async Task OnGet_NonThumbnailFileDoesNotExist_ReturnsNotfound()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var attachment = new AttachmentViewDto
        {
            Id = guid,
            FileName = ".pdf",
            FileExtension = ".pdf",
        };

        var complaintService = Substitute.For<IComplaintService>();
        complaintService.FindPublicAttachmentAsync(guid).Returns(attachment);

        var fileService = Substitute.For<IAttachmentFileService>();
        fileService.GetAttachmentFileAsync(Arg.Any<string>(), Arg.Any<bool>()).Returns([]);

        var pageModel = new AttachmentModel(complaintService, Substitute.For<IAttachmentFileService>());

        // Act
        var result = await pageModel.OnGetAsync(guid, attachment.FileName, thumbnail: false);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Test]
    public async Task OnGet_ThumbnailFileDoesNotExist_ReturnsRedirect()
    {
        var guid = Guid.NewGuid();
        var attachment = new AttachmentViewDto
        {
            Id = guid,
            FileName = $"{TextData.ShortName}.png",
            FileExtension = ".png",
        };

        var complaintService = Substitute.For<IComplaintService>();
        complaintService.FindPublicAttachmentAsync(guid).Returns(attachment);

        var fileService = Substitute.For<IAttachmentFileService>();
        fileService.GetAttachmentFileAsync(Arg.Any<string>(), Arg.Any<bool>()).Returns([]);

        var pageModel = new AttachmentModel(complaintService, Substitute.For<IAttachmentFileService>());

        // Act
        var result = await pageModel.OnGetAsync(guid, attachment.FileName, thumbnail: true);

        // Assert
        result.Should().BeOfType<LocalRedirectResult>();
    }
}
