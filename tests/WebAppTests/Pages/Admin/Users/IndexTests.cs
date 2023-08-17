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
        var officeServiceMock = Substitute.For<IOfficeService>();
        officeServiceMock.GetActiveListItemsAsync(CancellationToken.None)
            .Returns(new List<ListItem>());

        var paging = new PaginatedRequest(1, 1);
        var output = new PaginatedResult<StaffSearchResultDto>(new List<StaffSearchResultDto>(), 1, paging);
        var staffServiceMock = Substitute.For<IStaffService>();
        staffServiceMock.SearchAsync(Arg.Any<StaffSearchDto>(), Arg.Any<PaginatedRequest>(),
                Arg.Any<CancellationToken>())
            .Returns(output);

        var page = new IndexModel(officeServiceMock, staffServiceMock)
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
        var officeServiceMock = Substitute.For<IOfficeService>();
        officeServiceMock.GetActiveListItemsAsync(CancellationToken.None)
            .Returns(new List<ListItem>());
        var staffServiceMock = Substitute.For<IStaffService>();
        var page = new IndexModel(officeServiceMock, staffServiceMock)
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
