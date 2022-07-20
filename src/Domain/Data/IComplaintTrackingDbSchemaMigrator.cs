using System.Threading.Tasks;

namespace ComplaintTracking.Data;

public interface IComplaintTrackingDbSchemaMigrator
{
    Task MigrateAsync();
}
