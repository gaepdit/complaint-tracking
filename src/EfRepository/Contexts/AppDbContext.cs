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
    public DbSet<ActionType> ActionTypes { get; set; } = null!;
    public DbSet<Attachment> Attachments { get; set; } = null!;
    public DbSet<Complaint> Complaints { get; set; } = null!;
    public DbSet<ComplaintAction> ComplaintActions { get; set; } = null!;
    public DbSet<ComplaintTransition> ComplaintTransitions { get; set; } = null!;
    public DbSet<Concern> Concerns { get; set; } = null!;
    public DbSet<Office> Offices { get; set; } = null!;
    public DbSet<EmailLog> EmailLogs { get; set; } = null!;

    // Database views
    public DbSet<OpenComplaint> OpenComplaintsView { get; set; } = null!;
    public DbSet<ClosedComplaint> ClosedComplaintsView { get; set; } = null!;
    public DbSet<ClosedComplaintAction> ClosedComplaintActionsView { get; set; } = null!;
    public DbSet<RecordsCount> RecordsCountView { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder
            .ConfigureNavigationAutoIncludes()
            .ConfigureDatabaseViews()
            .ConfigureEnumValues()
            .ConfigureDateTimeOffsetHandling(Database.ProviderName)
            .ConfigurePerformanceIndexes()
            .ConfigureIdentityPasskeyData(Database.ProviderName);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
    }
}
