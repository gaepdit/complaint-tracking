using Cts.AppServices.ErrorLogging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
        catch (Exception ex)
        {
            ShortCode = await errorLogger.LogErrorAsync(ex);
        }
    }

    public void OnGetUnhandled()
    {
        throw new TestException("Test unhandled exception");
    }

    public class TestException(string message) : Exception(message);
}
