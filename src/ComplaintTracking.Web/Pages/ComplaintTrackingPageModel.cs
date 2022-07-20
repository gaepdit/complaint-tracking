using ComplaintTracking.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace ComplaintTracking.Web.Pages;

/* Inherit your PageModel classes from this class.
 */
public abstract class ComplaintTrackingPageModel : AbpPageModel
{
    protected ComplaintTrackingPageModel()
    {
        LocalizationResourceType = typeof(ComplaintTrackingResource);
    }
}
