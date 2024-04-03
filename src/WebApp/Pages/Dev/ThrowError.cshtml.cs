using Cts.AppServices.ErrorLogging;

namespace Cts.WebApp.Pages.Dev;

// Future: Remove this page once testing of error handling is complete.
[AllowAnonymous]
public class ThrowErrorModel(IErrorLogger errorLogger) : PageModel
{
    public string ShortCode { get; private set; } = string.Empty;

    public async Task OnGetAsync()
    {
        try
        {
            throw new TestException("Test handled exception");
        }
        catch (Exception e)
        {
            ShortCode = await errorLogger.LogErrorAsync(e);
        }
    }

    public void OnGetUnhandled()
    {
        throw new TestException("Test unhandled exception");
    }

    public class TestException(string message) : Exception(message);
}
