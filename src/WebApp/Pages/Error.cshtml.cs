using Cts.AppServices.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cts.WebApp.Pages;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
#pragma warning disable S4502 // Make sure disabling CSRF protection is safe here. 
[IgnoreAntiforgeryToken]
#pragma warning restore S4502
[AllowAnonymous]
public class ErrorModel(ILogger<ErrorModel> logger, IAuthorizationService authorizationService) : PageModel
{
    public int? Status { get; private set; }
    public bool ActiveUser { get; private set; }

    public async Task OnGet(int? statusCode)
    {
        ActiveUser = (await authorizationService.AuthorizeAsync(User, nameof(Policies.ActiveUser))).Succeeded;

        switch (statusCode)
        {
            case null:
                logger.LogError("Error page shown from Get method");
                break;
            case StatusCodes.Status404NotFound:
                logger.LogWarning("Error page shown from Get method with status code {StatusCode}", statusCode);
                break;
            default:
                logger.LogError("Error page shown from Get method with status code {StatusCode}", statusCode);
                break;
        }

        Status = statusCode;
    }

    public async Task OnPost()
    {
        ActiveUser = (await authorizationService.AuthorizeAsync(User, nameof(Policies.ActiveUser))).Succeeded;
        logger.LogError("Error page shown from Post method");
    }
}
