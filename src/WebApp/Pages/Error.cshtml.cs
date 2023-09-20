using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cts.WebApp.Pages;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
#pragma warning disable S4502 // Make sure disabling CSRF protection is safe here. 
[IgnoreAntiforgeryToken]
#pragma warning restore S4502
[AllowAnonymous]
public class ErrorModel : PageModel
{
    private readonly ILogger<ErrorModel> _logger;
    public ErrorModel(ILogger<ErrorModel> logger) => _logger = logger;

    public int? Status { get; private set; }

    public void OnGet(int? statusCode)
    {
        if (statusCode is null)
        {
            _logger.LogError("Error page shown from Get method");
        }
        else
        {
            _logger.LogError("Error page shown from Get method with status code {StatusCode}", statusCode);
        }

        Status = statusCode;
    }

    public void OnPost() => _logger.LogError("Error page shown from Post method");
}
