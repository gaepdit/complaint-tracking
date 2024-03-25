namespace Cts.WebApp.Pages.Account;

// This page is only used by the ExternalLogin page and is shown to users who have work accounts but are not allowed to sign in to the application.
[AllowAnonymous]
public class UnavailableModel : PageModel
{
    public void OnGet()
    {
        // Method intentionally left empty.
    }
}
