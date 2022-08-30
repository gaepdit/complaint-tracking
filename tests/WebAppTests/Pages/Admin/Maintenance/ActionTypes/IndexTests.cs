using Cts.AppServices.ActionTypes;
using Cts.TestData.ActionTypes;
using Cts.WebApp.Models;
using Cts.WebApp.Pages.Admin.Maintenance.ActionTypes;
using Cts.WebApp.Platform.RazorHelpers;

namespace WebAppTests.Pages.Admin.Maintenance.ActionTypes;

public class IndexTests
{
    private readonly List<ActionTypeViewDto> _list = new()
    {
        new ActionTypeViewDto { Id = Guid.Empty, Name = ActionTypeConstants.ValidName },
    };

    [Test]
    public async Task OnGet_ReturnsWithList()
    {
        var service = new Mock<IActionTypeAppService>();
        service.Setup(l => l.GetListAsync(CancellationToken.None)).ReturnsAsync(_list);
        var page = new IndexModel { TempData = WebAppTestsGlobal.GetPageTempData() };

        await page.OnGetAsync(service.Object);

        Assert.Multiple(() =>
        {
            page.Items.Should().BeEquivalentTo(_list);
            page.Message.Should().BeNull();
            page.HighlightId.Should().BeNull();
        });
    }

    [Test]
    public async Task SetDisplayMessage_ReturnsWithDisplayMessage()
    {
        var service = new Mock<IActionTypeAppService>();
        service.Setup(l => l.GetListAsync(CancellationToken.None)).ReturnsAsync(_list);
        var page = new IndexModel { TempData = WebAppTestsGlobal.GetPageTempData() };
        var expectedMessage = new DisplayMessage(DisplayMessage.AlertContext.Info, "Info message");

        page.TempData.SetDisplayMessage(expectedMessage.Context, expectedMessage.Message);
        await page.OnGetAsync(service.Object);

        page.Message.Should().BeEquivalentTo(expectedMessage);
    }
}
