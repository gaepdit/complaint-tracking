using Cts.AppServices.Permissions;
using Cts.AppServices.Permissions.Helpers;

namespace Cts.WebApp.Pages;

[AllowAnonymous]
public class SupportModel(IAuthorizationService authorization) : PageModel
{
    public bool ActiveUser { get; private set; }

    public async Task OnGetAsync() => ActiveUser = await authorization.Succeeded(User, Policies.ActiveUser);
}
