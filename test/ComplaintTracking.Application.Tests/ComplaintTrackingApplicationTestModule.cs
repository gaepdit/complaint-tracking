using Volo.Abp.Modularity;

namespace ComplaintTracking;

[DependsOn(
    typeof(ComplaintTrackingApplicationModule),
    typeof(ComplaintTrackingDomainTestModule)
    )]
public class ComplaintTrackingApplicationTestModule : AbpModule
{

}
