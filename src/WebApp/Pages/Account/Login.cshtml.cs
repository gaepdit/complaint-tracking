﻿namespace Cts.WebApp.Pages.Account;

[AllowAnonymous]
public class LoginModel : PageModel
{
    public string? ReturnUrl { get; private set; }

    public IActionResult OnGet(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated ?? false)
            return string.IsNullOrEmpty(returnUrl) ? RedirectToPage("/Index") : LocalRedirect(returnUrl);

        ReturnUrl = returnUrl;
        return Page();
    }
}
