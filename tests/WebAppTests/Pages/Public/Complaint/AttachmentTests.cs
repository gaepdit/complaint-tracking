using Cts.AppServices.Attachments;
using Cts.AppServices.Complaints;
using Cts.AppServices.Files;
using Cts.WebApp.Pages.Public.Complaints;

namespace WebAppTests.Pages.Public.Complaint;

public class AttachmentTests
{
    [Test]
    public async Task OnGet_AttachmentExistsAndFileNameMatches_ReturnsFile()
    {
        var item = new AttachmentPublicViewDto
        {
            FileName = TextData.ShortName,
            FileExtension = ".pdf",
            Id = Guid.Empty,
            IsImage = false,
            Size = 1,
        };

        var complaintServiceMock = Substitute.For<IComplaintService>();
        complaintServiceMock.FindPublicAttachmentAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(item);
        var fileServiceMock = Substitute.For<IFileService>();
        fileServiceMock.GetFileAsync(Arg.Any<string>(), Arg.Any<string?>())
            .Returns(new byte[] { 0x0 });
        var pageModel = new AttachmentModel();

        var result = await pageModel.OnGetAsync(complaintServiceMock, fileServiceMock,
            item.Id, item.FileName);

        using var scope = new AssertionScope();
        result.Should().BeOfType<FileContentResult>();
        ((FileContentResult)result).ContentType.Should().Be("application/pdf");
    }

    [Test]
    public async Task OnGet_AttachmentDoesNotExist_ReturnsNotfound()
    {
        var complaintServiceMock = Substitute.For<IComplaintService>();
        complaintServiceMock.FindPublicAttachmentAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((AttachmentPublicViewDto?)null);
        var pageModel = new AttachmentModel();

        var result = await pageModel.OnGetAsync(complaintServiceMock, Substitute.For<IFileService>(),
            Guid.Empty, TextData.ShortName);

        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Test]
    public async Task OnGet_FileNameDoesNotMatch_RedirectsToValidFileName()
    {
        var item = new AttachmentPublicViewDto
        {
            FileName = TextData.ValidName,
            FileExtension = ".pdf",
            Id = Guid.Empty,
            IsImage = false,
            Size = 0,
        };

        var complaintServiceMock = Substitute.For<IComplaintService>();
        complaintServiceMock.FindPublicAttachmentAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(item);
        var pageModel = new AttachmentModel();

        var result = await pageModel.OnGetAsync(complaintServiceMock, Substitute.For<IFileService>(),
            item.Id, TextData.NonExistentName);

        using var scope = new AssertionScope();
        result.Should().BeOfType<RedirectToPageResult>();
        ((RedirectToPageResult)result).PageName.Should().Be("Attachment");
    }

    [Test]
    public async Task OnGet_FileDoesNotExist_ReturnsNotfound()
    {
        var item = new AttachmentPublicViewDto
        {
            FileName = TextData.ShortName,
            FileExtension = ".pdf",
            Id = Guid.Empty,
            IsImage = false,
            Size = 0,
        };

        var complaintServiceMock = Substitute.For<IComplaintService>();
        complaintServiceMock.FindPublicAttachmentAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(item);
        var fileServiceMock = Substitute.For<IFileService>();
        fileServiceMock.GetFileAsync(Arg.Any<string>(), Arg.Any<string?>())
            .Returns(Array.Empty<byte>());
        var pageModel = new AttachmentModel();

        var result = await pageModel.OnGetAsync(complaintServiceMock, fileServiceMock,
            item.Id, item.FileName);

        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Test]
    public async Task OnGet_ImageFileDoesNotExist_ReturnsRedirect()
    {
        var item = new AttachmentPublicViewDto
        {
            FileName = TextData.ShortName,
            FileExtension = ".png",
            Id = Guid.Empty,
            IsImage = true,
            Size = 0,
        };

        var complaintServiceMock = Substitute.For<IComplaintService>();
        complaintServiceMock.FindPublicAttachmentAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(item);
        var fileServiceMock = Substitute.For<IFileService>();
        fileServiceMock.GetFileAsync(Arg.Any<string>(), Arg.Any<string?>())
            .Returns(Array.Empty<byte>());
        var pageModel = new AttachmentModel();

        var result = await pageModel.OnGetAsync(complaintServiceMock, fileServiceMock,
            item.Id, item.FileName);

        result.Should().BeOfType<RedirectResult>();
    }
}
