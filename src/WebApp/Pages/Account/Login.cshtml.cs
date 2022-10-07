using Cts.WebApp.Models;
using Cts.WebApp.Platform.RazorHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cts.WebApp.Pages.Account;

[AllowAnonymous]
public class LoginModel : PageModel
{
    public string? ReturnUrl { get; private set; }
    public DisplayMessage? Message { get; private set; }

    public IActionResult OnGet(string? returnUrl = null)
    {
        ReturnUrl = returnUrl ?? "/Index";

        if (User.Identity?.IsAuthenticated ?? false) return LocalRedirect(ReturnUrl);

        Message = TempData.GetDisplayMessage();
        return Page();
    }
}
