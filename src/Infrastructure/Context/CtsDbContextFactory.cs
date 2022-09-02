using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Cts.Infrastructure.Contexts
{
    /// <summary>
    /// Facilitates some EF Core Tools commands. See "Design-time DbContext Creation":
    /// https://docs.microsoft.com/en-us/ef/core/cli/dbcontext-creation?tabs=dotnet-core-cli#from-a-design-time-factory
    /// </summary>
    public class CtsDbContextFactory : IDesignTimeDbContextFactory<CtsDbContext>
    {
        public CtsDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CtsDbContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=cts-local;");
            return new CtsDbContext(optionsBuilder.Options);
        }
    }
}
