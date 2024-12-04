using Cts.Domain.DataViews;
using Cts.Domain.Entities.ActionTypes;
using Cts.Domain.Entities.Attachments;
using Cts.Domain.Entities.ComplaintActions;
using Cts.Domain.Entities.Complaints;
using Cts.Domain.Entities.ComplaintTransitions;
using Cts.Domain.Entities.Concerns;
using Cts.Domain.Entities.Offices;
using Cts.EfRepository.Contexts;
using Cts.EfRepository.DbConnection;
using Cts.EfRepository.Repositories;
using Cts.LocalRepository.Repositories;
using Cts.WebApp.Platform.Settings;
using GaEpd.EmailService.EmailLogRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Cts.WebApp.Platform.AppConfiguration;

public static class DataPersistence
{
    public static IServiceCollection AddDataPersistence(this IServiceCollection services,
        ConfigurationManager configuration)
    {
        // When configured, use in-memory data; otherwise use a SQL Server database.
        if (AppSettings.DevSettings.UseInMemoryData)
        {
            // Uses in-memory data.
            services.AddSingleton<IActionTypeRepository, LocalActionTypeRepository>();
            services.AddSingleton<IAttachmentRepository, LocalAttachmentRepository>();
            services.AddSingleton<IActionRepository, LocalActionRepository>();
            services.AddSingleton<IComplaintRepository, LocalComplaintRepository>();
            services.AddSingleton<IComplaintTransitionRepository, LocalComplaintTransitionRepository>();
            services.AddSingleton<IConcernRepository, LocalConcernRepository>();
            services.AddSingleton<IDataViewRepository, LocalDataViewRepository>();
            services.AddSingleton<IEmailLogRepository, LocalEmailLogRepository>();
            services.AddSingleton<IOfficeRepository, LocalOfficeRepository>();
        }
        else
        {
            // Uses a database connection.
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                // In-memory database (not recommended)
                services.AddDbContext<AppDbContext>(builder => builder.UseInMemoryDatabase("TEMP_DB"));
            }
            else
            {
                // Entity Framework context
                services.AddDbContext<AppDbContext>(dbContextOpts =>
                {
                    dbContextOpts.UseSqlServer(connectionString, sqlServerOpts => sqlServerOpts.EnableRetryOnFailure());
                    dbContextOpts.ConfigureWarnings(builder =>
                        builder.Throw(RelationalEventId.MultipleCollectionIncludeWarning));
                });

                // Dapper DB connection
                services.AddTransient<IDbConnectionFactory, DbConnectionFactory>(_ =>
                    new DbConnectionFactory(connectionString));
            }

            // Repositories
            services.AddScoped<IActionTypeRepository, ActionTypeRepository>();
            services.AddScoped<IAttachmentRepository, AttachmentRepository>();
            services.AddScoped<IActionRepository, ActionRepository>();
            services.AddScoped<IComplaintRepository, ComplaintRepository>();
            services.AddScoped<IComplaintTransitionRepository, ComplaintTransitionRepository>();
            services.AddScoped<IConcernRepository, ConcernRepository>();
            services.AddScoped<IDataViewRepository, DataViewRepository>();
            services.AddScoped<IEmailLogRepository, EmailLogRepository>();
            services.AddScoped<IOfficeRepository, OfficeRepository>();
        }

        return services;
    }
}
