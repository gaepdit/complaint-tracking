using GaEpd.AppLibrary.ListItems;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyAppRoot.AppServices.Offices;
using MyAppRoot.AppServices.Staff;
using MyAppRoot.WebApp.Pages.Admin.Users;

namespace WebAppTests.Pages.Admin.Users;

public class IndexTests
{
    [Test]
    public async Task OnSearch_IfValidModel_ReturnsPage()
    {
        var officeService = new Mock<IOfficeAppService>();
        officeService.Setup(l => l.GetActiveListItemsAsync(CancellationToken.None))
            .ReturnsAsync(new List<ListItem>());
        var staffService = new Mock<IStaffAppService>();
        staffService.Setup(l => l.GetListAsync(It.IsAny<StaffSearchDto>()))
            .ReturnsAsync(new List<StaffViewDto>());
        var page = new IndexModel(officeService.Object, staffService.Object)
            { TempData = WebAppTestsGlobal.GetPageTempData() };

        var result = await page.OnGetSearchAsync(new StaffSearchDto());

        Assert.Multiple(() =>
        {
            result.Should().BeOfType<PageResult>();
            page.ModelState.IsValid.Should().BeTrue();
            page.SearchResults.Should().BeEmpty();
            page.ShowResults.Should().BeTrue();
            page.HighlightId.Should().BeNull();
        });
    }

    [Test]
    public async Task OnSearch_IfInvalidModel_ReturnPageWithInvalidModelState()
    {
        var officeService = new Mock<IOfficeAppService>();
        officeService.Setup(l => l.GetActiveListItemsAsync(CancellationToken.None))
            .ReturnsAsync(new List<ListItem>());
        var staffService = new Mock<IStaffAppService>();
        var page = new IndexModel(officeService.Object, staffService.Object)
            { TempData = WebAppTestsGlobal.GetPageTempData() };
        page.ModelState.AddModelError("Error", "Sample error description");

        var result = await page.OnGetSearchAsync(new StaffSearchDto());

        Assert.Multiple(() =>
        {
            result.Should().BeOfType<PageResult>();
            page.ModelState.IsValid.Should().BeFalse();
        });
    }
}
