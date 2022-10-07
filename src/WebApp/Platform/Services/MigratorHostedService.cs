using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyAppRoot.Domain.Identity;
using MyAppRoot.Infrastructure.Contexts;
using MyAppRoot.TestData.SeedData;
using MyAppRoot.WebApp.Platform.Local;
using MyAppRoot.WebApp.Platform.Settings;

namespace MyAppRoot.WebApp.Platform.Services;

public class MigratorHostedService : IHostedService
{
    // Inject the IServiceProvider so we can create the DbContext scoped service.
    private readonly IServiceProvider _serviceProvider;
    public MigratorHostedService(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        // Retrieve scoped services.
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

        // If running on server, update the database.
        if (!env.IsLocalEnv())
        {
            // Run database migrations and add any new roles.
            await context.Database.MigrateAsync(cancellationToken);
            await AddNewRolesAsync(scope);
            return;
        }

        // If using in-memory data, no further action required.
        if (!ApplicationSettings.LocalDevSettings.BuildLocalDb) return;

        if (ApplicationSettings.LocalDevSettings.UseEfMigrations)
        {
            // Run any database migrations if used.
            await context.Database.MigrateAsync(cancellationToken);
            await AddNewRolesAsync(scope);
        }
        else
        {
            // Otherwise, delete and re-create the database.
            await context.Database.EnsureDeletedAsync(cancellationToken);
            await context.Database.EnsureCreatedAsync(cancellationToken);
        }

        // Add seed data to database.
        DbSeedDataHelpers.SeedAllData(context);

        async Task AddNewRolesAsync(IServiceScope serviceScope)
        {
            // Initialize any new roles.
            var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            foreach (var role in AppRole.AllRoles.Keys)
                if (!await context.Roles.AnyAsync(e => e.Name == role, cancellationToken))
                    await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // noop
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
