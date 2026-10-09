namespace Cts.WebApp.Pages.Content;

public class IndexModel : PageModel
{
    public IActionResult OnGet() => RedirectToPage("/Index");
}
