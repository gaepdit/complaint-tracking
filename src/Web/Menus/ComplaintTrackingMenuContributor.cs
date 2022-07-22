using ComplaintTracking.Localization;
using ComplaintTracking.MultiTenancy;
using Volo.Abp.Identity.Web.Navigation;
using Volo.Abp.SettingManagement.Web.Navigation;
using Volo.Abp.TenantManagement.Web.Navigation;
using Volo.Abp.UI.Navigation;

namespace ComplaintTracking.Web.Menus;

public class ComplaintTrackingMenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
    }

    private async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var administration = context.Menu.GetAdministration();
        var l = context.GetLocalizer<ComplaintTrackingResource>();

        if (!MultiTenancyConsts.IsEnabled) administration.TryRemoveMenuItem(TenantManagementMenuNames.GroupName);

        administration.SetSubItemOrder(IdentityMenuNames.GroupName, 2);
        administration.SetSubItemOrder(SettingManagementMenuNames.GroupName, 3);

        context.Menu.Items.Insert(0, new ApplicationMenuItem(ComplaintTrackingMenus.Home, l["Menu:Home"], "~/", icon: "fas fa-home", order: 0));
        context.Menu
            .AddItem(new ApplicationMenuItem(ComplaintTrackingMenus.Maintenance, l["Menu:Maintenance"], icon: "fa fa-wrench")
                .AddItem(new ApplicationMenuItem(ComplaintTrackingMenus.ActionTypes, l["Menu:ActionTypes"], url: "~/Maintenance/ActionTypes"))
                .AddItem(new ApplicationMenuItem(ComplaintTrackingMenus.Concerns, l["Menu:Concerns"], url: "~/Maintenance/Concerns"))
            );
    }
}
