using Cts.WebApp.Platform.OrgNotifications;

namespace Cts.WebApp.Pages.Shared.Components.Notifications;

public class NotificationsViewComponent(IOrgNotifications orgNotifications) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync() =>
        View("Default", await orgNotifications.GetOrgNotificationsAsync());
}
