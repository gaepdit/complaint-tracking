using Cts.AppServices.Offices;
using Cts.AppServices.Staff;
using Cts.WebApp.Pages.Admin.Users;
using FluentAssertions.Execution;
using GaEpd.AppLibrary.ListItems;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebAppTests.Pages.Admin.Users;

public class IndexTests
{
    [Test]
    public async Task OnSearch_IfValidModel_ReturnsPage()
    {
        var officeServiceMock = new Mock<IOfficeAppService>();
        officeServiceMock.Setup(l => l.GetActiveListItemsAsync(CancellationToken.None))
            .ReturnsAsync(new List<ListItem>());
        var staffServiceMock = new Mock<IStaffAppService>();
        staffServiceMock.Setup(l => l.GetListAsync(It.IsAny<StaffSearchDto>()))
            .ReturnsAsync(new List<StaffViewDto>());
        var page = new IndexModel(officeServiceMock.Object, staffServiceMock.Object)
            { TempData = WebAppTestsGlobal.PageTempData() };

        var result = await page.OnGetSearchAsync(new StaffSearchDto());

        using (new AssertionScope())
        {
            result.Should().BeOfType<PageResult>();
            page.ModelState.IsValid.Should().BeTrue();
            page.SearchResults.Should().BeEmpty();
            page.ShowResults.Should().BeTrue();
            page.HighlightId.Should().BeNull();
        }
    }

    [Test]
    public async Task OnSearch_IfInvalidModel_ReturnPageWithInvalidModelState()
    {
        var officeServiceMock = new Mock<IOfficeAppService>();
        officeServiceMock.Setup(l => l.GetActiveListItemsAsync(CancellationToken.None))
            .ReturnsAsync(new List<ListItem>());
        var staffServiceMock = new Mock<IStaffAppService>();
        var page = new IndexModel(officeServiceMock.Object, staffServiceMock.Object)
            { TempData = WebAppTestsGlobal.PageTempData() };
        page.ModelState.AddModelError("Error", "Sample error description");

        var result = await page.OnGetSearchAsync(new StaffSearchDto());

        using (new AssertionScope())
        {
            result.Should().BeOfType<PageResult>();
            page.ModelState.IsValid.Should().BeFalse();
        }
    }
}
