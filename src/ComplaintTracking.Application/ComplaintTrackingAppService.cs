using System;
using System.Collections.Generic;
using System.Text;
using ComplaintTracking.Localization;
using Volo.Abp.Application.Services;

namespace ComplaintTracking;

/* Inherit your application services from this class.
 */
public abstract class ComplaintTrackingAppService : ApplicationService
{
    protected ComplaintTrackingAppService()
    {
        LocalizationResource = typeof(ComplaintTrackingResource);
    }
}
