using Cts.AppServices.Notifications;
using Cts.Domain.DataViews.DataArchiveViews;
using Cts.Domain.Entities.ActionTypes;
using Cts.Domain.Entities.Attachments;
using Cts.Domain.Entities.ComplaintActions;
using Cts.Domain.Entities.Complaints;
using Cts.Domain.Entities.ComplaintTransitions;
using Cts.Domain.Entities.Concerns;
using Cts.Domain.Entities.Offices;
using Cts.Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Cts.EfRepository.Contexts;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    internal const string SqlServerProvider = "Microsoft.EntityFrameworkCore.SqlServer";
    internal const string SqliteProvider = "Microsoft.EntityFrameworkCore.Sqlite";

    // Domain entities
    public DbSet<ActionType> ActionTypes => Set<ActionType>();
    public DbSet<Attachment> Attachments => Set<Attachment>();
    public DbSet<Complaint> Complaints => Set<Complaint>();
    public DbSet<ComplaintAction> ComplaintActions => Set<ComplaintAction>();
    public DbSet<ComplaintTransition> ComplaintTransitions => Set<ComplaintTransition>();
    public DbSet<Concern> Concerns => Set<Concern>();
    public DbSet<Office> Offices => Set<Office>();
    public DbSet<EmailLog> EmailLogs => Set<EmailLog>();

    // Database views
    public DbSet<OpenComplaint> OpenComplaintsView => Set<OpenComplaint>();
    public DbSet<ClosedComplaint> ClosedComplaintsView => Set<ClosedComplaint>();
    public DbSet<ClosedComplaintAction> ClosedComplaintActionsView => Set<ClosedComplaintAction>();
    public DbSet<RecordsCount> RecordsCountView => Set<RecordsCount>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder
            .ConfigureNavigationAutoIncludes()
            .ConfigureDatabaseViews()
            .ConfigureEnumValues()
            .ConfigureDateTimeOffsetHandling(Database.ProviderName)
            .ConfigurePerformanceIndexes();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
    }
}
