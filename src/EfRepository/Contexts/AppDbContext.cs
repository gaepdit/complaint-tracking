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
        var appUserEntity = builder.Entity<ApplicationUser>();
        appUserEntity.Navigation(e => e.Office).AutoInclude();

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
        complaintEntity.Property(complaint => complaint.Status).HasConversion<string>();
        transitionEntity.Property(transition => transition.TransitionType).HasConversion<string>();

        // === Performance-related indexes
        // See https://learn.microsoft.com/en-us/ef/core/modeling/indexes?tabs=fluent-api#included-columns
        // and https://github.com/gaepdit/EPDDatabases/blob/16e12d19ae063a8df96226f55ba78107170bd86e/Troubleshooting_Scripts/ImprovePerformance/Microsoft/ComplaintTracking.sql
        // 
        // Some indexes cannot be added here because EF doesn't support indexing properties of complex/owned types.
        // These additional indexes are added directly in the EF migration file.
        // See https://github.com/dotnet/efcore/issues/31246#issuecomment-2836919642 for details.

#pragma warning disable S1192 // String literals should not be duplicated
        complaintEntity.HasIndex(["IsDeleted"], "missing_index_24_23");
        complaintEntity.HasIndex(["ComplaintCounty", "IsDeleted"], "missing_index_739_738");
        complaintEntity.HasIndex(["IsDeleted"], "missing_index_486_485")
            .IncludeProperties("PrimaryConcernId", "SecondaryConcernId");
        complaintEntity.HasIndex(["CurrentOwnerId", "CurrentOwnerAcceptedDate", "ComplaintClosed", "IsDeleted"],
            "missing_index_12_11");
        complaintEntity.HasIndex(["CurrentOfficeId", "CurrentOwnerAcceptedDate", "ComplaintClosed", "IsDeleted"],
            "missing_index_348_347");
        complaintEntity.HasIndex(["IsDeleted"], "missing_index_50_49").IncludeProperties("ReceivedDate");
        complaintEntity.HasIndex(["ComplaintCounty", "IsDeleted"], "missing_index_743_742")
            .IncludeProperties("PrimaryConcernId", "SecondaryConcernId");
        complaintEntity.HasIndex(["CurrentOfficeId", "ComplaintClosed", "IsDeleted", "CurrentOwnerId"],
                "missing_index_678_677")
            .IncludeProperties("Status", "ReceivedDate", "ComplaintCounty", "SourceFacilityName");
        complaintEntity.HasIndex(["ComplaintCounty", "IsDeleted"], "missing_index_1147_1146")
            .IncludeProperties("ComplaintNature", "ComplaintLocation", "ComplaintDirections");
        complaintEntity.HasIndex(["ComplaintCounty", "ComplaintClosed", "IsDeleted", "ComplaintClosedDate"],
            "missing_index_951_950");
        complaintEntity.HasIndex(["IsDeleted", "ComplaintCity"], "missing_index_1131_1130")
            .IncludeProperties("ComplaintNature", "ComplaintLocation", "ComplaintDirections");
        complaintEntity.HasIndex(["ComplaintCounty", "IsDeleted", "SourceFacilityName"], "missing_index_1260_1259");
        complaintEntity.HasIndex(["CurrentOfficeId", "IsDeleted", "CurrentOwnerId"], "missing_index_594_593")
            .IncludeProperties("Status", "ReceivedDate", "ComplaintCounty", "SourceFacilityName");
        complaintEntity.HasIndex(["ComplaintCounty", "IsDeleted"], "missing_index_22_21")
            .IncludeProperties("ReceivedDate");
        complaintEntity.HasIndex(["ReceivedById", "IsDeleted"], "missing_index_632_631");
        complaintEntity.HasIndex(["IsDeleted", "SourceContactName"], "missing_index_1208_1207");
        complaintEntity.HasIndex(["IsDeleted"], "missing_index_727_726")
            .IncludeProperties("ReceivedDate", "PrimaryConcernId", "SecondaryConcernId");
        complaintEntity.HasIndex(["IsDeleted", "SourceFacilityName"], "missing_index_55_54");
        complaintEntity.HasIndex(["CurrentOfficeId", "ComplaintClosed", "IsDeleted", "CurrentOwnerId"],
                "missing_index_690_689")
            .IncludeProperties("Status", "ReceivedDate", "ComplaintCounty", "SourceFacilityName",
                "ComplaintClosedDate");
        complaintEntity.HasIndex(["CurrentOfficeId", "CurrentOwnerId", "IsDeleted"], "missing_index_696_695");
        complaintEntity.HasIndex(["CurrentOfficeId", "IsDeleted"], "missing_index_734_733")
            .IncludeProperties("ReceivedDate", "PrimaryConcernId", "SecondaryConcernId");
        complaintEntity.HasIndex(["CurrentOfficeId", "CurrentOwnerId", "IsDeleted"], "missing_index_596_595")
            .IncludeProperties("ReceivedDate");
        actionEntity.HasIndex(["EnteredById", "IsDeleted", "EnteredDate"], "missing_index_1190_1189")
            .IncludeProperties("ComplaintId");
        actionEntity.HasIndex(["EnteredById", "IsDeleted", "EnteredDate"], "missing_index_1192_1191")
            .IncludeProperties("ComplaintId", "ActionTypeId", "ActionDate", "Investigator", "Comments", "CreatedAt",
                "CreatedById", "UpdatedAt", "UpdatedById", "DeletedAt", "DeletedById");
        appUserEntity.HasIndex(["ObjectIdentifier"], "missing_index_198_197");
#pragma warning restore S1192 // String literals should not be duplicated

        // === ## The following configurations are Sqlite only. ## ===
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
