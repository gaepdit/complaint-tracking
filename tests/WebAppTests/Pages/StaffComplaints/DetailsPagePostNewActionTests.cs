using Cts.AppServices.ComplaintActions.Dto;
using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.QueryDto;

namespace WebAppTests.Pages.StaffComplaints;

public class DetailsPagePostNewActionTests
{
    [Test]
    public async Task OnPostAsync_NullId_ReturnsRedirectToPageResult()
    {
        // Arrange
        var dto = new ActionCreateDto(1);
        var page = PageModelHelpers.BuildDetailsPageModel();

        // Act
        var result = await page.OnPostNewActionAsync(null, dto, CancellationToken.None);

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }

    [Test]
    public async Task OnPostAsync_ComplaintNotFound_ReturnsBadRequestResult()
    {
        // Arrange
        const int id = 0;
        var dto = new ActionCreateDto(id);
        var complaintService = Substitute.For<IComplaintService>();
        complaintService.FindAsync(id).Returns((ComplaintViewDto?)null);
        var page = PageModelHelpers.BuildDetailsPageModel(complaintService: complaintService);

        // Act
        var result = await page.OnPostNewActionAsync(id, dto, CancellationToken.None);

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }

    [Test]
    public async Task OnPostAsync_MismatchedId_ReturnsBadRequest()
    {
        // Arrange
        const int id = 0;
        var dto = new ActionCreateDto(id);
        var complaintService = Substitute.For<IComplaintService>();
        var page = PageModelHelpers.BuildDetailsPageModel(complaintService: complaintService);

        // Act
        var result = await page.OnPostNewActionAsync(999, dto, CancellationToken.None);

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }
}
