namespace WebAppTests.DisplayMessages;

public class DisplayMessageTests
{
    private class TestPage : PageModel;

    [Test]
    public void SetDisplayMessage_ReturnsWithDisplayMessage()
    {
        // Arrange
        var page = new TestPage { TempData = WebAppTestsSetup.PageTempData() };
        var expectedMessage = new DisplayMessage(DisplayMessage.AlertContext.Info, "Info message", []);

        // Act
        page.TempData.SetDisplayMessage(expectedMessage.Context, expectedMessage.Message);

        // Assert
        page.TempData.GetDisplayMessage().Should().BeEquivalentTo(expectedMessage);
    }
}
