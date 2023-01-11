using Cts.Domain.ActionTypes;
using Cts.Domain.Attachments;
using Cts.Domain.ComplaintActions;
using Cts.Domain.Complaints;
using Cts.Domain.ComplaintTransitions;
using Cts.Domain.Concerns;
using Cts.Domain.EmailLogs;
using Cts.Domain.Identity;
using Cts.Domain.Offices;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Cts.Infrastructure.Contexts;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Add domain entities here.

    public DbSet<ActionType> ActionTypes => Set<ActionType>();
    public DbSet<Attachment> Attachments => Set<Attachment>();
    public DbSet<Complaint> Complaints => Set<Complaint>();
    public DbSet<ComplaintAction> ComplaintActions => Set<ComplaintAction>();
    public DbSet<ComplaintTransition> ComplaintTransitions => Set<ComplaintTransition>();
    public DbSet<Concern> Concerns => Set<Concern>();
    public DbSet<Office> Offices => Set<Office>();
    public DbSet<EmailLog> EmailLogs => Set<EmailLog>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // See https://learn.microsoft.com/en-us/ef/core/querying/related-data/eager#model-configuration-for-auto-including-navigations
        builder.Entity<ApplicationUser>().Navigation(e => e.Office).AutoInclude();
        
        var complaintEntity = builder.Entity<Complaint>();
        complaintEntity.Navigation(e => e.PrimaryConcern).AutoInclude();
        complaintEntity.Navigation(e => e.SecondaryConcern).AutoInclude();
        complaintEntity.Navigation(e => e.CurrentOffice).AutoInclude();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution);
    }
}
