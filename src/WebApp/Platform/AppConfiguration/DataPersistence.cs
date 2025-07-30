using Cts.AppServices.Notifications;
using Cts.Domain.DataViews;
using Cts.Domain.Entities.ActionTypes;
using Cts.Domain.Entities.Attachments;
using Cts.Domain.Entities.ComplaintActions;
using Cts.Domain.Entities.Complaints;
using Cts.Domain.Entities.ComplaintTransitions;
using Cts.Domain.Entities.Concerns;
using Cts.Domain.Entities.Offices;
using Cts.Domain.Identity;
using Cts.EfRepository.Contexts;
using Cts.EfRepository.Contexts.SeedDevData;
using Cts.EfRepository.DbConnection;
using Cts.EfRepository.Repositories;
using Cts.LocalRepository.Repositories;
using Cts.WebApp.Platform.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Cts.WebApp.Platform.AppConfiguration;

public static class DataPersistence
{
    public static async Task ConfigureDataPersistence(this IHostApplicationBuilder builder)
    {
        if (AppSettings.DevSettings.UseDevSettings)
        {
            await builder.ConfigureDevDataPersistence();
            return;
        }

        builder.ConfigureDatabaseServices();

        await using var migrationContext = new AppDbContext(GetMigrationDbOpts(builder.Configuration).Options);
        await migrationContext.Database.MigrateAsync();
        await migrationContext.CreateMissingRolesAsync(builder.Services);
    }

    private static void ConfigureDatabaseServices(this IHostApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrEmpty(connectionString))
            throw new InvalidOperationException("No connection string found.");

        // Entity Framework context
        builder.Services.AddDbContext<AppDbContext>(db => db
            .UseSqlServer(connectionString, opts =>
            {
                opts.EnableRetryOnFailure();
                opts.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            })
            .ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.MultipleCollectionIncludeWarning)));

        // Dapper DB connection
        builder.Services.AddTransient<IDbConnectionFactory, DbConnectionFactory>(_ =>
            new DbConnectionFactory(connectionString));

        // Repositories
        builder.Services
            .AddScoped<IActionTypeRepository, ActionTypeRepository>()
            .AddScoped<IAttachmentRepository, AttachmentRepository>()
            .AddScoped<IActionRepository, ActionRepository>()
            .AddScoped<IComplaintRepository, ComplaintRepository>()
            .AddScoped<IComplaintTransitionRepository, ComplaintTransitionRepository>()
            .AddScoped<IConcernRepository, ConcernRepository>()
            .AddScoped<IDataViewRepository, DataViewRepository>()
            .AddScoped<IEmailLogRepository, EmailLogRepository>()
            .AddScoped<IOfficeRepository, OfficeRepository>();
    }

    private static DbContextOptionsBuilder<AppDbContext> GetMigrationDbOpts(IConfiguration configuration)
    {
        var migConnString = configuration.GetConnectionString("MigrationConnection");
        if (string.IsNullOrEmpty(migConnString))
            throw new InvalidOperationException("No migration connection string found.");

        return new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(migConnString, opts => opts.MigrationsAssembly(nameof(EfRepository)));
    }

    private static async Task CreateMissingRolesAsync(this AppDbContext migrationContext, IServiceCollection services)
    {
        // Initialize any new roles.
        var roleManager = services.BuildServiceProvider().GetRequiredService<RoleManager<IdentityRole>>();
        foreach (var role in AppRole.AllRoles.Keys)
            if (!await migrationContext.Roles.AnyAsync(idRole => idRole.Name == role))
                await roleManager.CreateAsync(new IdentityRole(role));
    }

    private static async Task ConfigureDevDataPersistence(this IHostApplicationBuilder builder)
    {
        // When configured, use in-memory data; otherwise use a SQL Server database.
        if (AppSettings.DevSettings.UseInMemoryData)
        {
            builder.Services
                .AddSingleton<IActionTypeRepository, LocalActionTypeRepository>()
                .AddSingleton<IAttachmentRepository, LocalAttachmentRepository>()
                .AddSingleton<IActionRepository, LocalActionRepository>()
                .AddSingleton<IComplaintRepository, LocalComplaintRepository>()
                .AddSingleton<IComplaintTransitionRepository, LocalComplaintTransitionRepository>()
                .AddSingleton<IConcernRepository, LocalConcernRepository>()
                .AddSingleton<IDataViewRepository, LocalDataViewRepository>()
                .AddSingleton<IEmailLogRepository, LocalEmailLogRepository>()
                .AddSingleton<IOfficeRepository, LocalOfficeRepository>();
        }
        else
        {
            builder.ConfigureDatabaseServices();

            await using var migrationContext = new AppDbContext(GetMigrationDbOpts(builder.Configuration).Options);
            await migrationContext.Database.EnsureDeletedAsync();

            if (AppSettings.DevSettings.UseEfMigrations)
                await migrationContext.Database.MigrateAsync();
            else
                await migrationContext.Database.EnsureCreatedAsync();

            DbSeedDataHelpers.SeedAllData(migrationContext);
        }
    }
}
