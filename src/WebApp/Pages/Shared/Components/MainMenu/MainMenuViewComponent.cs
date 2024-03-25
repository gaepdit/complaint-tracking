using Cts.AppServices.Permissions;
using Cts.WebApp.Pages.Account;
using System.Security.Claims;

namespace Cts.WebApp.Pages.Shared.Components.MainMenu;

public class MainMenuViewComponent(IAuthorizationService authorization) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(PageModel model) =>
        View("Default", new MenuParams
        {
            IsLoginPage = model is LoginModel,
            IsActiveUser = (await authorization.AuthorizeAsync((ClaimsPrincipal)User, Policies.ActiveUser)).Succeeded,
            IsStaffUser = (await authorization.AuthorizeAsync((ClaimsPrincipal)User, Policies.StaffUser)).Succeeded,
        });

    public record MenuParams
    {
        public bool IsLoginPage { get; init; }
        public bool IsActiveUser { get; init; }
        public bool IsStaffUser { get; init; }
    }
}
