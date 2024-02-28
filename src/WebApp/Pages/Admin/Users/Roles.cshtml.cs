using Cts.AppServices.Permissions;

namespace Cts.WebApp.Pages.Admin.Users;

[Authorize(Policy = nameof(Policies.ActiveUser))]
public class RolesModel : PageModel
{
    public void OnGet()
    {
        // Method intentionally left empty.
    }
}
