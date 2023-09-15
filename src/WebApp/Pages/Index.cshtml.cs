using Cts.AppServices.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cts.WebApp.Pages;

[AllowAnonymous]
public class IndexModel : PageModel
{
    // Constructor
    private readonly IAuthorizationService _authorization;
    public IndexModel(IAuthorizationService authorization) => _authorization = authorization;

    // Properties
    public bool ShowDashboard { get; private set; }

    // Methods
    public async Task<IActionResult> OnGetAsync()
    {
        ShowDashboard = await UseDashboardAsync();
        return Page();
    }

    private async Task<bool> UseDashboardAsync() =>
        (await _authorization.AuthorizeAsync(User, nameof(Policies.StaffUser))).Succeeded;
}
