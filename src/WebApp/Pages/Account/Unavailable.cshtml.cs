using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cts.WebApp.Pages.Account;

[AllowAnonymous]
public class UnavailableModel : PageModel
{
    public void OnGet()
    {
        // Method intentionally left empty.
    }
}
