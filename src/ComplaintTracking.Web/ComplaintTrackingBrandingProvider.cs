using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace ComplaintTracking.Web;

[Dependency(ReplaceServices = true)]
public class ComplaintTrackingBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "ComplaintTracking";
}
