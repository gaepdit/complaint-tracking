using Cts.AppServices.Offices;
using Cts.TestData.Constants;
using Cts.WebApp.Pages.Admin.Maintenance.Offices;
using Cts.WebApp.Platform.Models;
using Cts.WebApp.Platform.PageDisplayHelpers;
using FluentAssertions.Execution;

namespace WebAppTests.Pages.Admin.Maintenance.Offices;

public class IndexTests
{
    private static readonly List<OfficeViewDto> ListTest = new()
        { new OfficeViewDto { Id = Guid.Empty, Name = TestConstants.ValidName } };

    [Test]
    public async Task OnGet_ReturnsWithList()
    {
        var service = new Mock<IOfficeAppService>();
        service.Setup(l => l.GetListAsync(CancellationToken.None)).ReturnsAsync(ListTest);
        var page = new IndexModel { TempData = WebAppTestsGlobal.GetPageTempData() };

        await page.OnGetAsync(service.Object);

        using (new AssertionScope())
        {
            page.Items.Should().BeEquivalentTo(ListTest);
            page.Message.Should().BeNull();
            page.HighlightId.Should().BeNull();
        }
    }

    [Test]
    public async Task SetDisplayMessage_ReturnsWithDisplayMessage()
    {
        var service = new Mock<IOfficeAppService>();
        service.Setup(l => l.GetListAsync(CancellationToken.None)).ReturnsAsync(ListTest);
        var page = new IndexModel { TempData = WebAppTestsGlobal.GetPageTempData() };
        var expectedMessage = new DisplayMessage(DisplayMessage.AlertContext.Info, "Info message");

        page.TempData.SetDisplayMessage(expectedMessage.Context, expectedMessage.Message);
        await page.OnGetAsync(service.Object);

        page.Message.Should().BeEquivalentTo(expectedMessage);
    }
}
