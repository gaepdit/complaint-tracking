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
    public int? Status { get; private set; }

    private readonly ILogger<ErrorModel> _logger;
    public ErrorModel(ILogger<ErrorModel> logger) => _logger = logger;

    public void OnGet(int? statusCode)
    {
        _logger.LogError("Error page Get method accessed {StatusCode}", statusCode);
        Status = statusCode;
    }

    public void OnPost() => _logger.LogError("Error page Post method accessed");
}
