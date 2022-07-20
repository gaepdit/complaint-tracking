using ComplaintTracking.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace ComplaintTracking.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(ComplaintTrackingEntityFrameworkCoreModule),
    typeof(ComplaintTrackingApplicationContractsModule)
    )]
public class ComplaintTrackingDbMigratorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);
    }
}
