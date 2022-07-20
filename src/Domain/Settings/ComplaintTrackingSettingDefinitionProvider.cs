using Volo.Abp.Settings;

namespace ComplaintTracking.Settings;

public class ComplaintTrackingSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(ComplaintTrackingSettings.MySetting1));
    }
}
