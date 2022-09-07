using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cts.WebApp.Pages.Account;

[AllowAnonymous]
public class Unavailable : PageModel
{
    public static void OnGet()
    {
        // Method intentionally left empty.
    }
}
