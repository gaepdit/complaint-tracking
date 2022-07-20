using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ComplaintTracking.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class ComplaintTrackingDbContextFactory : IDesignTimeDbContextFactory<ComplaintTrackingDbContext>
{
    public ComplaintTrackingDbContext CreateDbContext(string[] args)
    {
        ComplaintTrackingEfCoreEntityExtensionMappings.Configure();

        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<ComplaintTrackingDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));

        return new ComplaintTrackingDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../ComplaintTracking.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
