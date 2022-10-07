using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyAppRoot.WebApp.Pages.Admin.Maintenance;

[Authorize]
public class IndexModel : PageModel
{
    public void OnGet()
    {
        // Method intentionally left empty.
    }
}
