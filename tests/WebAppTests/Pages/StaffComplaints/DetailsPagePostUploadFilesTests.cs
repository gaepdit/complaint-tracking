using Cts.AppServices.Attachments.Dto;
using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.QueryDto;

namespace WebAppTests.Pages.StaffComplaints;

public class DetailsPagePostUploadFilesTests
{
    [Test]
    public async Task OnPostAsync_DefaultId_ReturnsRedirectToPageResult()
    {
        // Arrange
        var page = PageModelHelpers.BuildDetailsPageModel();

        // Act
        var result = await page.OnPostUploadFilesAsync(new AttachmentsUploadDto(), CancellationToken.None);

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }

    [Test]
    public async Task OnPostAsync_ComplaintNotFound_ReturnsBadRequestResult()
    {
        // Arrange
        var complaintService = Substitute.For<IComplaintService>();
        complaintService.FindAsync(Arg.Any<int>()).Returns((ComplaintViewDto?)null);
        var page = PageModelHelpers.BuildDetailsPageModel(complaintService: complaintService);
        page.Id = 1_000_000;

        // Act
        var result = await page.OnPostUploadFilesAsync(new AttachmentsUploadDto(), CancellationToken.None);

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }

    [Test]
    public async Task OnPostAsync_InvalidId_ReturnsBadRequest()
    {
        // Arrange
        var complaintService = Substitute.For<IComplaintService>();
        var page = PageModelHelpers.BuildDetailsPageModel(complaintService: complaintService);
        page.Id = -1;

        // Act
        var result = await page.OnPostUploadFilesAsync(new AttachmentsUploadDto(), CancellationToken.None);

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }
}
