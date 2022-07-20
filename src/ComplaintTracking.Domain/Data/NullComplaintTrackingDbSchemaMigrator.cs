using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace ComplaintTracking.Data;

/* This is used if database provider does't define
 * IComplaintTrackingDbSchemaMigrator implementation.
 */
public class NullComplaintTrackingDbSchemaMigrator : IComplaintTrackingDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
