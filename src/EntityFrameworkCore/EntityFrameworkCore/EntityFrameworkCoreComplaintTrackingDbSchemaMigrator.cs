using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ComplaintTracking.Data;
using Volo.Abp.DependencyInjection;

namespace ComplaintTracking.EntityFrameworkCore;

public class EntityFrameworkCoreComplaintTrackingDbSchemaMigrator
    : IComplaintTrackingDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreComplaintTrackingDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the ComplaintTrackingDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<ComplaintTrackingDbContext>()
            .Database
            .MigrateAsync();
    }
}
