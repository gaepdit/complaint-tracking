using ComplaintTracking.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace ComplaintTracking.Permissions;

public class ComplaintTrackingPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(ComplaintTrackingPermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(ComplaintTrackingPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ComplaintTrackingResource>(name);
    }
}
