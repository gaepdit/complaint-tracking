using Cts.AppServices.Permissions;

namespace Cts.WebApp.Pages;

[AllowAnonymous]
public class SupportModel(IAuthorizationService authorizationService) : PageModel
{
    public bool ActiveUser { get; private set; }
    public string? Version { get; private set; }

    public async Task OnGetAsync()
    {
        ActiveUser = (await authorizationService.AuthorizeAsync(User, nameof(Policies.ActiveUser))).Succeeded;
        Version = GetType().Assembly.GetName().Version?.ToString(fieldCount: 3);
    }
}
