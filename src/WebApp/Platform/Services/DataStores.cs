using Cts.AppServices.Files;
using Cts.Domain.Entities.ActionTypes;
using Cts.Domain.Entities.Complaints;
using Cts.Domain.Entities.Concerns;
using Cts.Domain.Entities.Offices;
using Cts.EfRepository.Contexts;
using Cts.EfRepository.Repositories;
using Cts.LocalRepository.Files;
using Cts.LocalRepository.Repositories;
using Cts.WebApp.Platform.Settings;
using Microsoft.EntityFrameworkCore;

namespace Cts.WebApp.Platform.Services;

public static class DataStores
{
    public static void AddDataStores(this IServiceCollection services, ConfigurationManager configuration)
    {
        // When configured, use in-memory data; otherwise use a SQL Server database.
        if (ApplicationSettings.DevSettings.UseInMemoryData)
        {
            // Uses local static data if no database is built.
            services.AddSingleton<IActionTypeRepository, LocalActionTypeRepository>();
            services.AddSingleton<IComplaintRepository, LocalComplaintRepository>();
            services.AddSingleton<IConcernRepository, LocalConcernRepository>();
            services.AddSingleton<IOfficeRepository, LocalOfficeRepository>();
        }
        else
        {
            string? connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                services.AddDbContext<AppDbContext>(opts => opts.UseInMemoryDatabase("TEMP_DB"));
            }
            else
            {
                services.AddDbContext<AppDbContext>(opts =>
                    opts.UseSqlServer(connectionString, x => x.MigrationsAssembly("EfRepository")));
            }

            services.AddScoped<IActionTypeRepository, ActionTypeRepository>();
            services.AddScoped<IComplaintRepository, ComplaintRepository>();
            services.AddScoped<IConcernRepository, ConcernRepository>();
            services.AddScoped<IOfficeRepository, OfficeRepository>();
        }

        // When running locally, you have the option to access file in memory or use the local filesystem.
        if (ApplicationSettings.DevSettings.UseInMemoryFiles)
        {
            services.AddTransient<IFileService, InMemoryFileService>();
        }
        else
        {
            services.AddTransient<IFileService,
                FileSystemFileService>(_ => new FileSystemFileService(configuration["PersistedFilesBasePath"] ?? ""));
        }
    }
}
