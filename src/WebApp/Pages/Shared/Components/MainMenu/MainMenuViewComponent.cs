using Cts.AppServices.Permissions;
using Cts.WebApp.Pages.Account;
using System.Security.Claims;

namespace Cts.WebApp.Pages.Shared.Components.MainMenu;

public class MainMenuViewComponent(IAuthorizationService authorization) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(PageModel model) =>
        View("Default", new MenuParams
        {
            HideSignin = model is LoginModel,
            IsActive = (await authorization.AuthorizeAsync((ClaimsPrincipal)User, Policies.ActiveUser)).Succeeded,
            IsLoggedIn = (await authorization.AuthorizeAsync((ClaimsPrincipal)User, Policies.LoggedInUser)).Succeeded,
            IsStaff = (await authorization.AuthorizeAsync((ClaimsPrincipal)User, Policies.StaffUser)).Succeeded,
        });

    public record MenuParams
    {
        public bool HideSignin { get; init; }
        public bool IsActive { get; init; }
        public bool IsLoggedIn { get; init; }
        public bool IsStaff { get; init; }
    }
}
