using Cts.AppServices.Permissions;

namespace Cts.WebApp.Pages;

[AllowAnonymous]
public class IndexModel(IAuthorizationService authorization) : PageModel
{
    public bool ShowDashboard { get; private set; }

    public async Task<IActionResult> OnGetAsync()
    {
        ShowDashboard = await UseDashboardAsync();
        return Page();
    }

    private async Task<bool> UseDashboardAsync() =>
        (await authorization.AuthorizeAsync(User, nameof(Policies.StaffUser))).Succeeded;
}
