using Cts.Domain.DataViews.DataArchiveViews;
using Cts.Domain.Entities.ActionTypes;
using Cts.Domain.Entities.Attachments;
using Cts.Domain.Entities.ComplaintActions;
using Cts.Domain.Entities.Complaints;
using Cts.Domain.Entities.ComplaintTransitions;
using Cts.Domain.Entities.Concerns;
using Cts.Domain.Entities.Offices;
using Cts.Domain.Identity;
using GaEpd.EmailService.EmailLogRepository;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Cts.EfRepository.Contexts;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    internal const string SqlServerProvider = "Microsoft.EntityFrameworkCore.SqlServer";
    private const string SqliteProvider = "Microsoft.EntityFrameworkCore.Sqlite";

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

        // === Auto-includes ===
        // Some properties should always be included.
        // See https://learn.microsoft.com/en-us/ef/core/querying/related-data/eager#model-configuration-for-auto-including-navigations

        // Users
        builder.Entity<ApplicationUser>().Navigation(e => e.Office).AutoInclude();

        // Attachments
        builder.Entity<Attachment>().Navigation(e => e.UploadedBy).AutoInclude();

        // Complaints
        var complaintEntity = builder.Entity<Complaint>();
        complaintEntity.Navigation(complaint => complaint.PrimaryConcern).AutoInclude();
        complaintEntity.Navigation(complaint => complaint.SecondaryConcern).AutoInclude();
        complaintEntity.Navigation(complaint => complaint.CurrentOffice).AutoInclude();
        complaintEntity.Navigation(complaint => complaint.EnteredBy).AutoInclude();
        complaintEntity.Navigation(complaint => complaint.ReceivedBy).AutoInclude();
        complaintEntity.Navigation(complaint => complaint.CurrentOwner).AutoInclude();
        complaintEntity.Navigation(complaint => complaint.ReviewedBy).AutoInclude();
        complaintEntity.Navigation(complaint => complaint.DeletedBy).AutoInclude();

        // Complaint Action
        var actionEntity = builder.Entity<ComplaintAction>();
        actionEntity.Navigation(action => action.ActionType).AutoInclude();
        actionEntity.Navigation(action => action.EnteredBy).AutoInclude();

        // Complaint Transition
        var transitionEntity = builder.Entity<ComplaintTransition>();
        transitionEntity.Navigation(transition => transition.CommittedByUser).AutoInclude();
        transitionEntity.Navigation(transition => transition.TransferredToOffice).AutoInclude();
        transitionEntity.Navigation(transition => transition.TransferredToUser).AutoInclude();

        // === Database views ===
        // See https://khalidabuhakmeh.com/how-to-add-a-view-to-an-entity-framework-core-dbcontext

        builder.Entity<OpenComplaint>().HasNoKey().ToView(nameof(OpenComplaintsView));
        builder.Entity<ClosedComplaint>().HasNoKey().ToView(nameof(ClosedComplaintsView));
        builder.Entity<ClosedComplaintAction>().HasNoKey().ToView(nameof(ClosedComplaintActionsView));
        builder.Entity<RecordsCount>().HasNoKey().ToView(nameof(RecordsCountView));

        // === Additional configuration ===

        // Let's save enums in the database as strings.
        // See https://learn.microsoft.com/en-us/ef/core/modeling/value-conversions?tabs=data-annotations#pre-defined-conversions
        builder.Entity<Complaint>().Property(complaint => complaint.Status).HasConversion<string>();
        builder.Entity<ComplaintTransition>().Property(transition => transition.TransitionType).HasConversion<string>();

        // ## The following configurations are Sqlite only. ##
        if (Database.ProviderName != SqliteProvider) return;

#pragma warning disable S125 // Sections of code should not be commented out
        // Sqlite and EF Core are in conflict on how to handle collections of owned types.
        // See: https://stackoverflow.com/a/69826156/212978
        // and: https://learn.microsoft.com/en-us/ef/core/modeling/owned-entities#collections-of-owned-types
        // builder.Entity<EntityWithOwnedTypeCollection>().OwnsMany(e => e.OwnedTypeCollection, b => b.HasKey("Id"));
        // === UNUSED because CTS does not have any entities with collections of owned types.
#pragma warning restore S125

        // "Handling DateTimeOffset in SQLite with Entity Framework Core"
        // https://blog.dangl.me/archive/handling-datetimeoffset-in-sqlite-with-entity-framework-core/
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            var dateTimeOffsetProperties = entityType.ClrType.GetProperties()
                .Where(info =>
                    info.PropertyType == typeof(DateTimeOffset) || info.PropertyType == typeof(DateTimeOffset?));
            foreach (var property in dateTimeOffsetProperties)
                builder.Entity(entityType.Name).Property(property.Name)
                    .HasConversion(new DateTimeOffsetToBinaryConverter());
        }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
    }
}
