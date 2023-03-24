using Cts.AppServices.ActionTypes;
using Cts.TestData.Constants;
using Cts.WebApp.Pages.Admin.Maintenance.ActionTypes;
using Cts.WebApp.Platform.Models;
using Cts.WebApp.Platform.PageModelHelpers;
using FluentAssertions.Execution;

namespace WebAppTests.Pages.Admin.Maintenance.ActionTypes;

public class IndexTests
{
    private static readonly List<ActionTypeViewDto> ListTest = new()
        { new ActionTypeViewDto { Id = Guid.Empty, Name = TestConstants.ValidName } };

    [Test]
    public async Task OnGet_ReturnsWithList()
    {
        var serviceMock = new Mock<IActionTypeAppService>();
        serviceMock.Setup(l => l.GetListAsync(CancellationToken.None)).ReturnsAsync(ListTest);
        var page = new IndexModel { TempData = WebAppTestsGlobal.GetPageTempData() };

        await page.OnGetAsync(serviceMock.Object);

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
        var serviceMock = new Mock<IActionTypeAppService>();
        serviceMock.Setup(l => l.GetListAsync(CancellationToken.None)).ReturnsAsync(ListTest);
        var page = new IndexModel { TempData = WebAppTestsGlobal.GetPageTempData() };
        var expectedMessage = new DisplayMessage(DisplayMessage.AlertContext.Info, "Info message");

        page.TempData.SetDisplayMessage(expectedMessage.Context, expectedMessage.Message);
        await page.OnGetAsync(serviceMock.Object);

        page.Message.Should().BeEquivalentTo(expectedMessage);
    }
}
