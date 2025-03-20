using Cts.AppServices.ComplaintActions.Dto;
using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.QueryDto;

namespace WebAppTests.Pages.StaffComplaints;

public class DetailsPagePostNewActionTests
{
    [Test]
    public async Task OnPostAsync_DefaultId_ReturnsRedirectToPageResult()
    {
        // Arrange
        var dto = new ActionCreateDto(1);
        var page = PageModelHelpers.BuildDetailsPageModel();

        // Act
        var result = await page.OnPostNewActionAsync(dto, CancellationToken.None);

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
        page.Id = id;

        // Act
        var result = await page.OnPostNewActionAsync(dto, CancellationToken.None);

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }

    [Test]
    public async Task OnPostAsync_MismatchedId_ReturnsBadRequest()
    {
        // Arrange
        const int id = 2;
        var dto = new ActionCreateDto(id);
        var complaintService = Substitute.For<IComplaintService>();
        var page = PageModelHelpers.BuildDetailsPageModel(complaintService: complaintService);
        page.Id = 1;

        // Act
        var result = await page.OnPostNewActionAsync(dto, CancellationToken.None);

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }
}
