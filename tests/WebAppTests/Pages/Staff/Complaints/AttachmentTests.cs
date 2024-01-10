using Cts.AppServices.Attachments;
using Cts.AppServices.Complaints;
using Cts.AppServices.Files;
using Cts.WebApp.Pages.Staff.Complaints;

namespace WebAppTests.Pages.Staff.Complaints;

[TestFixture]
public class AttachmentTests
{
    private AttachmentModel _pageModel = null!;

    [SetUp]
    public void Setup() => _pageModel = new AttachmentModel();

    [Test]
    public async Task OnGet_AttachmentExistsAndFileNameMatches_ReturnsFile()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var fileBytes = new byte[] { 0x0 };
        var attachment = new AttachmentViewDto
        {
            Id = guid,
            FileName = $"{TextData.ShortName}.pdf",
            FileExtension = ".pdf",
        };

        var complaintService = Substitute.For<IComplaintService>();
        complaintService.FindAttachmentAsync(guid).Returns(attachment);

        var fileService = Substitute.For<IFileService>();
        fileService.GetFileAsync(Arg.Any<string>(), Arg.Any<string?>()).Returns(fileBytes);

        // Act
        var result = await _pageModel.OnGetAsync(complaintService, fileService, guid, attachment.FileName);

        // Assert
        result.Should().BeOfType<FileContentResult>();
        ((FileContentResult)result).FileContents.Should().BeEquivalentTo(fileBytes);
    }

    [Test]
    public async Task OnGet_AttachmentDoesNotExist_ReturnsNotfound()
    {
        var guid = Guid.NewGuid();
        var complaintService = Substitute.For<IComplaintService>();
        complaintService.FindAttachmentAsync(guid).Returns((AttachmentViewDto?)null);
        
        var fileService = Substitute.For<IFileService>();

        // Act
        var result = await _pageModel.OnGetAsync(complaintService, fileService, guid, TextData.ShortName);

        result.Should().BeOfType<NotFoundObjectResult>();
    }
    [Test]
    public async Task OnGet_FileNameDoesNotMatch_RedirectsToValidFileName()
    {
        var guid = Guid.NewGuid();
        var attachment = new AttachmentViewDto
        {
            Id = guid,
            FileName = $"{TextData.ValidName}.pdf",
            FileExtension = ".pdf",
        };

        var complaintService = Substitute.For<IComplaintService>();
        complaintService.FindAttachmentAsync(guid).Returns(attachment);
        
        var fileService = Substitute.For<IFileService>();

        // Act
        var result = await _pageModel.OnGetAsync(complaintService, fileService, guid, TextData.NonExistentName);

        using var scope = new AssertionScope();
        result.Should().BeOfType<RedirectToPageResult>();
        ((RedirectToPageResult)result).PageName.Should().Be("Attachment");
    }

    [Test]
    public async Task OnGet_NonImageFileDoesNotExist_ReturnsNotfound()
    {
        var guid = Guid.NewGuid();
        var attachment = new AttachmentViewDto
        {
            Id = guid,
            FileName = $"{TextData.ShortName}.pdf",
            FileExtension = ".pdf",
        };
        
        var complaintService = Substitute.For<IComplaintService>();
        complaintService.FindAttachmentAsync(guid).Returns(attachment);
        
        var fileService = Substitute.For<IFileService>();
        fileService.GetFileAsync(Arg.Any<string>(), Arg.Any<string?>()).Returns(Array.Empty<byte>());

        // Act
        var result = await _pageModel.OnGetAsync(complaintService, fileService, guid, attachment.FileName);

        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Test]
    public async Task OnGet_ImageFileDoesNotExist_ReturnsRedirect()
    {
        var guid = Guid.NewGuid();
        var attachment = new AttachmentViewDto
        {
            Id = guid,
            FileName = $"{TextData.ShortName}.png",
            FileExtension = ".png",
        };
        
        var complaintService = Substitute.For<IComplaintService>();
        complaintService.FindAttachmentAsync(guid).Returns(attachment);
        
        var fileService = Substitute.For<IFileService>();
        fileService.GetFileAsync(Arg.Any<string>(), Arg.Any<string?>()).Returns(Array.Empty<byte>());

        // Act
        var result = await _pageModel.OnGetAsync(complaintService, fileService, guid, attachment.FileName);

        result.Should().BeOfType<RedirectResult>();
    }
}
