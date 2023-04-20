using Cts.Domain.Entities.ActionTypes;
using Cts.Domain.Entities.Attachments;
using Cts.Domain.Entities.ComplaintActions;
using Cts.Domain.Entities.Complaints;
using Cts.Domain.Entities.ComplaintTransitions;
using Cts.Domain.Entities.Concerns;
using Cts.Domain.Entities.EmailLogs;
using Cts.Domain.Entities.Offices;
using Cts.Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Cts.EfRepository.Contexts;

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
        builder.Entity<Attachment>().Navigation(e => e.UploadedBy).AutoInclude();
        
        var complaint = builder.Entity<Complaint>();
        complaint.Navigation(e => e.PrimaryConcern).AutoInclude();
        complaint.Navigation(e => e.SecondaryConcern).AutoInclude();
        complaint.Navigation(e => e.CurrentOffice).AutoInclude();
        complaint.Navigation(e => e.EnteredBy).AutoInclude();
        complaint.Navigation(e => e.ReceivedBy).AutoInclude();
        complaint.Navigation(e => e.CurrentOwner).AutoInclude();
        complaint.Navigation(e => e.ReviewedBy).AutoInclude();
        complaint.Navigation(e => e.DeletedBy).AutoInclude();

        var action = builder.Entity<ComplaintAction>();
        action.Navigation(e => e.ActionType).AutoInclude();
        action.Navigation(e => e.EnteredBy).AutoInclude();

        var transition = builder.Entity<ComplaintTransition>();
        transition.Navigation(e => e.CommittedByUser).AutoInclude();
        transition.Navigation(e => e.TransferredFromOffice).AutoInclude();
        transition.Navigation(e => e.TransferredFromUser).AutoInclude();
        transition.Navigation(e => e.TransferredToOffice).AutoInclude();
        transition.Navigation(e => e.TransferredToUser).AutoInclude();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
    }
}
