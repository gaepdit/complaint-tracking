using Cts.Domain.Identity;
using Cts.Infrastructure.Contexts;
using Cts.WebApp.Platform.Local;
using Cts.WebApp.Platform.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Cts.WebApp.Platform.Services;

public class MigratorHostedService : IHostedService
{
    // Inject the IServiceProvider so we can create the DbContext scoped service.
    private readonly IServiceProvider _serviceProvider;
    public MigratorHostedService(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        // Create a new scope to retrieve scoped services.
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<CtsDbContext>();
        var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

        // Initialize database if used.
        if (!env.IsLocalEnv() || ApplicationSettings.LocalDevSettings.BuildLocalDb)
        {
            if (!env.IsLocalEnv() || ApplicationSettings.LocalDevSettings.UseEfMigrations)
            {
                // Run any database migrations.
                await context.Database.MigrateAsync(cancellationToken);
            }
            else
            {
                // Delete and re-create the database.
                await context.Database.EnsureDeletedAsync(cancellationToken);
                await context.Database.EnsureCreatedAsync(cancellationToken);
            }
        }

        if (env.IsLocalEnv() && ApplicationSettings.LocalDevSettings.BuildLocalDb)
        {
            // TODO: Seed data in LocalDB
        }

        // Initialize any new roles.
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        foreach (var role in AppRole.AllRoles.Keys)
            if (!await context.Roles.AnyAsync(e => e.Name == role, cancellationToken))
                await roleManager.CreateAsync(new IdentityRole(role));
    }

    // noop
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
