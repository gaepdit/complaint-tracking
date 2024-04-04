using Cts.WebApp.Pages.Account;

namespace WebAppTests.DisplayMessages;

public class DisplayMessageTests
{
    [Test]
    public void SetDisplayMessage_ReturnsWithDisplayMessage()
    {
        // Arrange
        // The actual Page model here doesn't matter. DisplayMessage is available for all pages.
        var page = new UnavailableModel { TempData = WebAppTestsSetup.PageTempData() };
        var expectedMessage = new DisplayMessage(DisplayMessage.AlertContext.Info, "Info message", []);
        page.TempData.SetDisplayMessage(expectedMessage.Context, expectedMessage.Message);

        // Act
        page.OnGet();

        // Assert
        page.TempData.GetDisplayMessage().Should().BeEquivalentTo(expectedMessage);
    }
}
