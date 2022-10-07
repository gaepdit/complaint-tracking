using Cts.Domain.Identity;
using Cts.Domain.Offices;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Cts.Infrastructure.Contexts;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Add domain entities here.
    public DbSet<Office> Offices => Set<Office>();
}
