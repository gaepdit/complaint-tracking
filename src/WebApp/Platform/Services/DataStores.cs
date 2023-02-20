using Cts.AppServices.Files;
using Cts.Domain.ActionTypes;
using Cts.Domain.Complaints;
using Cts.Domain.Concerns;
using Cts.Domain.Offices;
using Cts.EfRepository.Contexts;
using Cts.EfRepository.Repositories;
using Cts.LocalRepository.Files;
using Cts.LocalRepository.Repositories;
using Cts.WebApp.Platform.Settings;
using Microsoft.EntityFrameworkCore;

namespace Cts.WebApp.Platform.Services;

public static class DataStores
{
    public static void AddDataStores(this IServiceCollection services,
        ConfigurationManager configuration, bool isLocal)
    {
        // When running locally, you have the option to use in-memory data or a database.
        if (isLocal && ApplicationSettings.LocalDevSettings.UseInMemoryData)
        {
            services.AddDbContext<AppDbContext>(opts =>
                opts.UseInMemoryDatabase("TEMP_DB"));

            // Uses local static data if no database is built.
            services.AddSingleton<IActionTypeRepository, LocalActionTypeRepository>();
            services.AddSingleton<IComplaintRepository, LocalComplaintRepository>();
            services.AddSingleton<IConcernRepository, LocalConcernRepository>();
            services.AddSingleton<IOfficeRepository, LocalOfficeRepository>();
        }
        else
        {
            services.AddDbContext<AppDbContext>(opts =>
                opts.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    x => x.MigrationsAssembly("EfRepository")));


            services.AddScoped<IActionTypeRepository, ActionTypeRepository>();
            services.AddScoped<IComplaintRepository, ComplaintRepository>();
            services.AddScoped<IConcernRepository, ConcernRepository>();
            services.AddScoped<IOfficeRepository, OfficeRepository>();
        }

        // When running locally, you have the option to access file in memory or use the local filesystem.
        if (isLocal && ApplicationSettings.LocalDevSettings.UseInMemoryFiles)
        {
            services.AddTransient<IFileService, InMemoryFileService>();
        }
        else
        {
            services.AddTransient<IFileService,
                FileSystemFileService>(_ => new FileSystemFileService(configuration["PersistedFilesBasePath"]));
        }
    }
}
