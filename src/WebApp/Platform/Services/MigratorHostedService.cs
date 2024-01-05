using Cts.Domain.Identity;
using Cts.EfRepository.Contexts;
using Cts.EfRepository.Contexts.SeedDevData;
using Cts.WebApp.Platform.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Cts.WebApp.Platform.Services;

public class MigratorHostedService(IServiceProvider serviceProvider, IConfiguration configuration) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        // If using in-memory data, no further action required.
        if (ApplicationSettings.DevSettings.UseInMemoryData) return;

        // Inject the IServiceProvider so we can create the DbContext scoped service.
        using var scope = serviceProvider.CreateScope();

        var migrationConnectionString = configuration.GetConnectionString("MigrationConnection");
        var migrationOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(migrationConnectionString, builder => builder.MigrationsAssembly("EfRepository"))
            .Options;

        await using var migrationContext = new AppDbContext(migrationOptions);

        if (ApplicationSettings.DevSettings.UseEfMigrations)
        {
            // Run any EF database migrations if used.
            await migrationContext.Database.MigrateAsync(cancellationToken);

            // Initialize any new roles. (No other data is seeded when running EF migrations.)
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            foreach (var role in AppRole.AllRoles.Keys)
                if (!await migrationContext.Roles.AnyAsync(e => e.Name == role, cancellationToken))
                    await roleManager.CreateAsync(new IdentityRole(role));
        }
        else
        {
            // Otherwise, delete and re-create the database.
            await migrationContext.Database.EnsureDeletedAsync(cancellationToken);
            await migrationContext.Database.EnsureCreatedAsync(cancellationToken);

            // Add seed data to database.
            DbSeedDataHelpers.SeedAllData(migrationContext);
        }
    }

    // noop
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
