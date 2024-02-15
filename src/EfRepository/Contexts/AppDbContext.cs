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
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Cts.EfRepository.Contexts;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    internal const string SqlServerProvider = "Microsoft.EntityFrameworkCore.SqlServer";
    private const string SqliteProvider = "Microsoft.EntityFrameworkCore.Sqlite";

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

        // Some properties should always be included.
        // See https://learn.microsoft.com/en-us/ef/core/querying/related-data/eager#model-configuration-for-auto-including-navigations
        
        // Users
        builder.Entity<ApplicationUser>().Navigation(e => e.Office).AutoInclude();

        // Attachments
        builder.Entity<Attachment>().Navigation(e => e.UploadedBy).AutoInclude();

        // Complaints
        var complaint = builder.Entity<Complaint>();
        complaint.Navigation(e => e.PrimaryConcern).AutoInclude();
        complaint.Navigation(e => e.SecondaryConcern).AutoInclude();
        complaint.Navigation(e => e.CurrentOffice).AutoInclude();
        complaint.Navigation(e => e.EnteredBy).AutoInclude();
        complaint.Navigation(e => e.ReceivedBy).AutoInclude();
        complaint.Navigation(e => e.CurrentOwner).AutoInclude();
        complaint.Navigation(e => e.ReviewedBy).AutoInclude();
        complaint.Navigation(e => e.DeletedBy).AutoInclude();

        // Complaint Action
        var action = builder.Entity<ComplaintAction>();
        action.Navigation(e => e.ActionType).AutoInclude();
        action.Navigation(e => e.EnteredBy).AutoInclude();

        // Complaint Transition
        var transition = builder.Entity<ComplaintTransition>();
        transition.Navigation(e => e.CommittedByUser).AutoInclude();
        transition.Navigation(e => e.TransferredFromOffice).AutoInclude();
        transition.Navigation(e => e.TransferredFromUser).AutoInclude();
        transition.Navigation(e => e.TransferredToOffice).AutoInclude();
        transition.Navigation(e => e.TransferredToUser).AutoInclude();

#pragma warning disable S125
        // Let's save enums in the database as strings.
        // See https://stackoverflow.com/a/55260541/212978
        // builder.Entity<EntityWithEnum>().Property(d => d.MyEnum).HasConversion(new EnumToStringConverter<MyEnumType>());
#pragma warning restore S125

        // ## The following configurations are Sqlite only. ##
        if (Database.ProviderName != SqliteProvider) return;

#pragma warning disable S125
        // Sqlite and EF Core are in conflict on how to handle collections of owned types.
        // See: https://stackoverflow.com/a/69826156/212978
        // and: https://learn.microsoft.com/en-us/ef/core/modeling/owned-entities#collections-of-owned-types
        // builder.Entity<EntityWithOwnedType>().OwnsMany(e => e.MyOwnedTypeProperty, b => b.HasKey("Id"));
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
