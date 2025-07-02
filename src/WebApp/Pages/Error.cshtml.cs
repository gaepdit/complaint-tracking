using Cts.AppServices.AuthorizationPolicies;

namespace Cts.WebApp.Pages;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[AllowAnonymous]
public class ErrorModel(IAuthorizationService authorization) : PageModel
{
    public int? Status { get; private set; }
    public bool ActiveUser { get; private set; }

    public async Task OnGetAsync(int? statusCode)
    {
        ActiveUser = await authorization.Succeeded(User, Policies.ActiveUser);
        Status = statusCode;
    }
}
