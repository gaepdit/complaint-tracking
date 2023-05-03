using Cts.AppServices.Offices;
using Cts.AppServices.Staff;
using Cts.AppServices.Staff.Dto;
using Cts.WebApp.Pages.Admin.Users;
using FluentAssertions.Execution;
using GaEpd.AppLibrary.ListItems;
using GaEpd.AppLibrary.Pagination;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebAppTests.Pages.Admin.Users;

public class IndexTests
{
    [Test]
    public async Task OnSearch_IfValidModel_ReturnsPage()
    {
        // Arrange
        var officeServiceMock = new Mock<IOfficeService>();
        officeServiceMock.Setup(l => l.GetActiveListItemsAsync(CancellationToken.None))
            .ReturnsAsync(new List<ListItem>());

        var paging = new PaginatedRequest(1, 1);
        var output = new PaginatedResult<StaffSearchResultDto>(new List<StaffSearchResultDto>(), 1, paging);
        var staffServiceMock = new Mock<IStaffService>();
        staffServiceMock.Setup(l =>
                l.SearchAsync(It.IsAny<StaffSearchDto>(), It.IsAny<PaginatedRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(output);

        var page = new IndexModel(officeServiceMock.Object, staffServiceMock.Object)
            { TempData = WebAppTestsSetup.PageTempData() };

        // Act
        var result = await page.OnGetSearchAsync(new StaffSearchDto());

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeOfType<PageResult>();
            page.ModelState.IsValid.Should().BeTrue();
            page.SearchResults.Should().Be(output);
            page.SearchResults.Items.Should().BeEmpty();
            page.ShowResults.Should().BeTrue();
            page.HighlightId.Should().BeNull();
        }
    }

    [Test]
    public async Task OnSearch_IfInvalidModel_ReturnPageWithInvalidModelState()
    {
        var officeServiceMock = new Mock<IOfficeService>();
        officeServiceMock.Setup(l => l.GetActiveListItemsAsync(CancellationToken.None))
            .ReturnsAsync(new List<ListItem>());
        var staffServiceMock = new Mock<IStaffService>();
        var page = new IndexModel(officeServiceMock.Object, staffServiceMock.Object)
            { TempData = WebAppTestsSetup.PageTempData() };
        page.ModelState.AddModelError("Error", "Sample error description");

        var result = await page.OnGetSearchAsync(new StaffSearchDto());

        using (new AssertionScope())
        {
            result.Should().BeOfType<PageResult>();
            page.ModelState.IsValid.Should().BeFalse();
        }
    }
}
