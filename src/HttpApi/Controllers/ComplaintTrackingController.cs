using ComplaintTracking.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace ComplaintTracking.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class ComplaintTrackingController : AbpControllerBase
{
    protected ComplaintTrackingController()
    {
        LocalizationResource = typeof(ComplaintTrackingResource);
    }
}
