using Cts.Domain.Entities.ActionTypes;
using Cts.Domain.Entities.Attachments;
using Cts.Domain.Entities.ComplaintActions;
using Cts.Domain.Entities.Complaints;
using Cts.Domain.Entities.ComplaintTransitions;
using Cts.Domain.Entities.Concerns;
using Cts.Domain.Entities.Offices;
using Cts.EfRepository.Contexts;
using Cts.EfRepository.Repositories;
using Cts.LocalRepository.Repositories;
using Cts.WebApp.Platform.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Cts.WebApp.Platform.Services;

public static class DataPersistence
{
    public static void AddDataPersistence(this IServiceCollection services, ConfigurationManager configuration)
    {
        // When configured, use in-memory data; otherwise use a SQL Server database.
        if (AppSettings.DevSettings.UseInMemoryData)
        {
            // Uses local static data if no database is built.
            services.AddSingleton<IActionTypeRepository, LocalActionTypeRepository>();
            services.AddSingleton<IAttachmentRepository, LocalAttachmentRepository>();
            services.AddSingleton<IActionRepository, LocalActionRepository>();
            services.AddSingleton<IComplaintRepository, LocalComplaintRepository>();
            services.AddSingleton<IComplaintTransitionRepository, LocalComplaintTransitionRepository>();
            services.AddSingleton<IConcernRepository, LocalConcernRepository>();
            services.AddSingleton<IOfficeRepository, LocalOfficeRepository>();
        }
        else
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                services.AddDbContext<AppDbContext>(opts => opts.UseInMemoryDatabase("TEMP_DB"));
            }
            else
            {
                services.AddDbContext<AppDbContext>(dbContextOpts =>
                {
                    dbContextOpts.UseSqlServer(connectionString, sqlServerOpts => sqlServerOpts.EnableRetryOnFailure());
                    dbContextOpts.ConfigureWarnings(builder =>
                        builder.Throw(RelationalEventId.MultipleCollectionIncludeWarning));
                });
            }

            services.AddScoped<IActionTypeRepository, ActionTypeRepository>();
            services.AddScoped<IAttachmentRepository, AttachmentRepository>();
            services.AddScoped<IActionRepository, ActionRepository>();
            services.AddScoped<IComplaintRepository, ComplaintRepository>();
            services.AddScoped<IComplaintTransitionRepository, ComplaintTransitionRepository>();
            services.AddScoped<IConcernRepository, ConcernRepository>();
            services.AddScoped<IOfficeRepository, OfficeRepository>();
        }
    }
}
