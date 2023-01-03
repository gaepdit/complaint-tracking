using Cts.WebApp.Platform.Models;
using Cts.WebApp.Platform.PageDisplayHelpers;
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
        if (User.Identity?.IsAuthenticated ?? false) 
            return string.IsNullOrEmpty(returnUrl) ? RedirectToPage("/Index") : LocalRedirect(returnUrl);
            
        ReturnUrl = returnUrl;

        Message = TempData.GetDisplayMessage();
        return Page();
    }
}
