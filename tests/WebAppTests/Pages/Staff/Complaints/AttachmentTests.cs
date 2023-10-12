using Cts.AppServices.Attachments;
using Cts.AppServices.Complaints;
using Cts.AppServices.Files;
using Cts.WebApp.Pages.Staff.Complaints;

namespace WebAppTests.Pages.Staff.Complaints;

[TestFixture]
public class AttachmentTests
{
    private IComplaintService _complaintService = null!;
    private AttachmentModel _pageModel = null!;

    [SetUp]
    public void Setup()
    {
        _complaintService = Substitute.For<IComplaintService>();
        _pageModel = new AttachmentModel();
    }

    [TearDown]
    public void Teardown()
    {
        _complaintService.Dispose();
    }

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

        _complaintService.FindAttachmentAsync(guid).Returns(attachment);
        var fileService = Substitute.For<IFileService>();
        fileService.GetFileAsync(Arg.Any<string>(), Arg.Any<string?>()).Returns(fileBytes);

        // Act
        var result = await _pageModel.OnGetAsync(_complaintService, fileService, guid, attachment.FileName);

        // Assert
        result.Should().BeOfType<FileContentResult>();
        ((FileContentResult)result).FileContents.Should().BeEquivalentTo(fileBytes);
    }

    [Test]
    public async Task OnGet_AttachmentDoesNotExist_ReturnsNotfound()
    {
        var guid = Guid.NewGuid();
        _complaintService.FindAttachmentAsync(guid).Returns((AttachmentViewDto?)null);
        var fileService = Substitute.For<IFileService>();

        var result = await _pageModel.OnGetAsync(_complaintService, fileService, guid, TextData.ShortName);

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

        _complaintService.FindAttachmentAsync(guid).Returns(attachment);
        var fileService = Substitute.For<IFileService>();

        var result = await _pageModel.OnGetAsync(_complaintService, fileService, guid, TextData.NonExistentName);

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
        _complaintService.FindAttachmentAsync(guid).Returns(attachment);
        var fileService = Substitute.For<IFileService>();
        fileService.GetFileAsync(Arg.Any<string>(), Arg.Any<string?>()).Returns(Array.Empty<byte>());

        var result = await _pageModel.OnGetAsync(_complaintService, fileService, guid, attachment.FileName);

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
        _complaintService.FindAttachmentAsync(guid).Returns(attachment);
        var fileService = Substitute.For<IFileService>();
        fileService.GetFileAsync(Arg.Any<string>(), Arg.Any<string?>()).Returns(Array.Empty<byte>());

        var result = await _pageModel.OnGetAsync(_complaintService, fileService, guid, attachment.FileName);

        result.Should().BeOfType<RedirectResult>();
    }
}
