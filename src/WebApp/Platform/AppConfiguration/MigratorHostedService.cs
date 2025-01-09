using Cts.Domain.Identity;
using Cts.EfRepository.Contexts;
using Cts.EfRepository.Contexts.SeedDevData;
using Cts.TestData;
using Cts.WebApp.Platform.Settings;
using GaEpd.FileService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Cts.WebApp.Platform.AppConfiguration;

public class MigratorHostedService(IServiceProvider serviceProvider, IConfiguration configuration) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();

        if (AppSettings.DevSettings.UseDevSettings) await SeedFileStoreAsync(scope, cancellationToken);

        // If using in-memory data store, no further action required.
        if (AppSettings.DevSettings.UseInMemoryData) return;

        var migrationConnectionString = configuration.GetConnectionString("MigrationConnection");
        var dbContextOptionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(migrationConnectionString, builder =>
            {
                builder.MigrationsAssembly("EfRepository");
                builder.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds);
            });

        if (AppSettings.DevSettings.UseDevSettings)
        {
            dbContextOptionsBuilder
                .LogTo(Console.WriteLine, [DbLoggerCategory.Database.Command.Name], LogLevel.Information);
        }

        await using var migrationContext = new AppDbContext(dbContextOptionsBuilder.Options);

        if (AppSettings.DevSettings.UseEfMigrations)
        {
            // Run any EF database migrations if used.
            await migrationContext.Database.MigrateAsync(cancellationToken);

            // Initialize any new roles. (No other data is seeded when running EF migrations.)
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            foreach (var role in AppRole.AllRoles.Keys)
            {
                if (!await migrationContext.Roles.AnyAsync(idRole => idRole.Name == role, cancellationToken))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
        else if (AppSettings.DevSettings.DeleteAndRebuildDatabase)
        {
            // Delete and re-create the database.
            await migrationContext.Database.EnsureDeletedAsync(cancellationToken);
            await migrationContext.Database.EnsureCreatedAsync(cancellationToken);

            // Add seed data to database.
            DbSeedDataHelpers.SeedAllData(migrationContext);
        }
    }

    // Initialize the attachments file store
    private static async Task SeedFileStoreAsync(IServiceScope scope, CancellationToken cancellationToken)
    {
        var fileService = scope.ServiceProvider.GetRequiredService<IFileService>();

        foreach (var attachment in AttachmentData.GetAttachmentFiles())
        {
            var fileBytes = attachment.Base64EncodedFile == null
                ? []
                : Convert.FromBase64String(attachment.Base64EncodedFile);

            await using var fileStream = new MemoryStream(fileBytes);
            await fileService.SaveFileAsync(fileStream, attachment.FileName, attachment.Path, token: cancellationToken);
        }
    }

    // noop
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
