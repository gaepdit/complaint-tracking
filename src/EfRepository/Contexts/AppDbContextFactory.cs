using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Design;

namespace Cts.EfRepository.Contexts;

/// <summary>
/// Facilitates some EF Core Tools commands. See "Design-time DbContext Creation":
/// https://docs.microsoft.com/en-us/ef/core/cli/dbcontext-creation?tabs=dotnet-core-cli#from-a-design-time-factory
/// </summary>
[UsedImplicitly]
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=cts-app;",
            // FUTURE: This will no longer be necessary after upgrading to .NET 8.
            builder => builder.UseDateOnlyTimeOnly());
        return new AppDbContext(optionsBuilder.Options);
    }
}
