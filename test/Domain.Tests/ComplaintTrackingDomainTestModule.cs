using ComplaintTracking.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace ComplaintTracking;

[DependsOn(
    typeof(ComplaintTrackingEntityFrameworkCoreTestModule)
    )]
public class ComplaintTrackingDomainTestModule : AbpModule
{

}
