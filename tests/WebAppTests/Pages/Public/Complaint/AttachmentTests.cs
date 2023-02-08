using Cts.AppServices.Attachments;
using Cts.AppServices.Complaints;
using Cts.AppServices.Files;
using Cts.TestData.Constants;
using Cts.WebApp.Pages.Public.Complaint;
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
            FileName = TestConstants.ShortName,
            FileExtension = ".pdf",
            Id = Guid.Empty,
            IsImage = false,
            Size = 1,
        };

        var complaintServiceMock = new Mock<IComplaintAppService>();
        complaintServiceMock.Setup(l => l.GetPublicAttachmentAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(item);
        var fileServiceMock = new Mock<IFileService>();
        fileServiceMock.Setup(l => l.GetFileAsync(It.IsAny<string>(), It.IsAny<string?>()))
            .ReturnsAsync(new byte[] { 0x20 });
        var pageModel = new AttachmentModel();

        var result = await pageModel.OnGetAsync(complaintServiceMock.Object, fileServiceMock.Object,
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
        var complaintServiceMock = new Mock<IComplaintAppService>();
        complaintServiceMock.Setup(l => l.GetPublicAttachmentAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((AttachmentPublicViewDto?)null);
        var pageModel = new AttachmentModel();

        var result = await pageModel.OnGetAsync(complaintServiceMock.Object, Mock.Of<IFileService>(),
            Guid.Empty, TestConstants.ShortName);

        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Test]
    public async Task IncorrectFilename_ReturnsRedirect()
    {
        var item = new AttachmentPublicViewDto
        {
            FileName = TestConstants.ValidName,
            FileExtension = ".pdf",
            Id = Guid.Empty,
            IsImage = false,
            Size = 0,
        };

        var complaintServiceMock = new Mock<IComplaintAppService>();
        complaintServiceMock.Setup(l => l.GetPublicAttachmentAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(item);
        var pageModel = new AttachmentModel();

        var result = await pageModel.OnGetAsync(complaintServiceMock.Object, Mock.Of<IFileService>(),
            item.Id, TestConstants.NonExistentName);

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
            FileName = TestConstants.ShortName,
            FileExtension = ".pdf",
            Id = Guid.Empty,
            IsImage = false,
            Size = 0,
        };

        var complaintServiceMock = new Mock<IComplaintAppService>();
        complaintServiceMock.Setup(l => l.GetPublicAttachmentAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(item);
        var fileServiceMock = new Mock<IFileService>();
        fileServiceMock.Setup(l => l.GetFileAsync(It.IsAny<string>(), It.IsAny<string?>()))
            .ReturnsAsync(Array.Empty<byte>());
        var pageModel = new AttachmentModel();

        var result = await pageModel.OnGetAsync(complaintServiceMock.Object, fileServiceMock.Object,
            item.Id, item.FileName);

        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Test]
    public async Task EmptyImageFile_ReturnsRedirect()
    {
        var item = new AttachmentPublicViewDto
        {
            FileName = TestConstants.ShortName,
            FileExtension = ".png",
            Id = Guid.Empty,
            IsImage = true,
            Size = 0,
        };

        var complaintServiceMock = new Mock<IComplaintAppService>();
        complaintServiceMock.Setup(l => l.GetPublicAttachmentAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(item);
        var fileServiceMock = new Mock<IFileService>();
        fileServiceMock.Setup(l => l.GetFileAsync(It.IsAny<string>(), It.IsAny<string?>()))
            .ReturnsAsync(Array.Empty<byte>());
        var pageModel = new AttachmentModel();

        var result = await pageModel.OnGetAsync(complaintServiceMock.Object, fileServiceMock.Object,
            item.Id, item.FileName);

        result.Should().BeOfType<RedirectResult>();
    }
}
