using Cts.AppServices.Attachments;
using Cts.AppServices.Attachments.Dto;
using Cts.WebApp.Pages;

namespace WebAppTests.Pages.PublicComplaints;

public class PublicAttachmentPageTests
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
            FileName = TextData.ValidPdfFileName,
            FileExtension = TextData.ValidPdfFileExtension,
        };

        var attachmentService = Substitute.For<IAttachmentService>();
        attachmentService.FindPublicAttachmentAsync(guid).Returns(attachment);
        attachmentService.GetAttachmentFileAsync(Arg.Any<string>(), Arg.Any<bool>(),
                Arg.Any<IAttachmentService.AttachmentServiceConfig>())
            .Returns(fileBytes);

        var pageModel = new AttachmentModel();

        // Act
        var result = await pageModel.OnGetAsync(attachmentService, guid, attachment.FileName);

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

        var attachmentService = Substitute.For<IAttachmentService>();
        attachmentService.FindPublicAttachmentAsync(guid).Returns((AttachmentViewDto?)null);

        var pageModel = new AttachmentModel();

        // Act
        var result = await pageModel.OnGetAsync(attachmentService, guid, TextData.ShortName);

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
            FileName = TextData.ValidPdfFileName,
            FileExtension = TextData.ValidPdfFileExtension,
        };

        var attachmentService = Substitute.For<IAttachmentService>();
        attachmentService.FindPublicAttachmentAsync(guid).Returns(attachment);

        var pageModel = new AttachmentModel();

        // Act
        var result = await pageModel.OnGetAsync(attachmentService, guid, TextData.NonExistentName);

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
            FileName = TextData.ValidPdfFileName,
            FileExtension = TextData.ValidPdfFileExtension,
        };

        var attachmentService = Substitute.For<IAttachmentService>();
        attachmentService.FindPublicAttachmentAsync(guid).Returns(attachment);
        attachmentService.GetAttachmentFileAsync(Arg.Any<string>(), Arg.Any<bool>(),
            Arg.Any<IAttachmentService.AttachmentServiceConfig>()).Returns([]);

        var pageModel = new AttachmentModel();

        // Act
        var result = await pageModel.OnGetAsync(attachmentService, guid, attachment.FileName, thumbnail: false);

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

        var attachmentService = Substitute.For<IAttachmentService>();
        attachmentService.FindPublicAttachmentAsync(guid).Returns(attachment);
        attachmentService.GetAttachmentFileAsync(Arg.Any<string>(), Arg.Any<bool>(),
            Arg.Any<IAttachmentService.AttachmentServiceConfig>()).Returns([]);

        var pageModel = new AttachmentModel();

        // Act
        var result = await pageModel.OnGetAsync(attachmentService, guid, attachment.FileName, thumbnail: true);

        // Assert
        result.Should().BeOfType<LocalRedirectResult>();
    }
}
