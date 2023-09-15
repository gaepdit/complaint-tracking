using Cts.AppServices.Attachments;
using Cts.AppServices.Complaints;
using Cts.AppServices.Files;
using Cts.TestData.Constants;
using Cts.WebApp.Pages.Public.Complaints;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;

namespace WebAppTests.Pages.Public.Complaint;

public class AttachmentTests
{
    [Test]
    public async Task OnGet_PopulatesThePageModel()
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
            .Returns(new byte[] { 0x20 });
        var pageModel = new AttachmentModel();

        var result = await pageModel.OnGetAsync(complaintServiceMock, fileServiceMock,
            item.Id, item.FileName);

        using (new AssertionScope())
        {
            result.Should().BeOfType<FileContentResult>();
            ((FileContentResult)result).ContentType.Should().Be("application/pdf");
        }
    }

    [Test]
    public async Task NonexistentItem_ReturnsNotFound()
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
    public async Task IncorrectFilename_ReturnsRedirect()
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

        using (new AssertionScope())
        {
            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult)result).PageName.Should().Be("Attachment");
        }
    }

    [Test]
    public async Task EmptyNonImageFile_ReturnsNotFound()
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
    public async Task EmptyImageFile_ReturnsRedirect()
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
