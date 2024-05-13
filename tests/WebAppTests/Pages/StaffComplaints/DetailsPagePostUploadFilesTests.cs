using Cts.AppServices.Attachments.Dto;
using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.QueryDto;

namespace WebAppTests.Pages.StaffComplaints;

public class DetailsPagePostUploadFilesTests
{
    [Test]
    public async Task OnPostAsync_NullId_ReturnsRedirectToPageResult()
    {
        // Arrange
        var page = PageModelHelpers.BuildDetailsPageModel();

        // Act
        var result = await page.OnPostUploadFilesAsync(null, new AttachmentsUploadDto(), CancellationToken.None);

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }

    [Test]
    public async Task OnPostAsync_ComplaintNotFound_ReturnsBadRequestResult()
    {
        // Arrange
        const int id = 0;
        var complaintService = Substitute.For<IComplaintService>();
        complaintService.FindAsync(id).Returns((ComplaintViewDto?)null);
        var page = PageModelHelpers.BuildDetailsPageModel(complaintService: complaintService);

        // Act
        var result = await page.OnPostUploadFilesAsync(id, new AttachmentsUploadDto(), CancellationToken.None);

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }

    [Test]
    public async Task OnPostAsync_MismatchedId_ReturnsBadRequest()
    {
        // Arrange
        const int id = 999;
        var complaintService = Substitute.For<IComplaintService>();
        var page = PageModelHelpers.BuildDetailsPageModel(complaintService: complaintService);

        // Act
        var result = await page.OnPostUploadFilesAsync(id, new AttachmentsUploadDto(), CancellationToken.None);

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }
}
