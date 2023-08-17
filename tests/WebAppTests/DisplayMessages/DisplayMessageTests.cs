using Cts.WebApp.Pages;
using Cts.WebApp.Platform.Models;
using Cts.WebApp.Platform.PageModelHelpers;
using Microsoft.Extensions.Logging;

namespace WebAppTests.DisplayMessages;

public class DisplayMessageTests
{
    [Test]
    public void SetDisplayMessage_ReturnsWithDisplayMessage()
    {
        // Arrange
        // The actual page model here doesn't matter. DisplayMessage is available for all pages.
        var page = new ErrorModel(Substitute.For<ILogger<ErrorModel>>()) { TempData = WebAppTestsSetup.PageTempData() };
        var expectedMessage = new DisplayMessage(DisplayMessage.AlertContext.Info, "Info message");
        page.TempData.SetDisplayMessage(expectedMessage.Context, expectedMessage.Message);

        // Act
        page.OnGet(null);

        // Assert
        page.TempData.GetDisplayMessage().Should().BeEquivalentTo(expectedMessage);
    }
}
