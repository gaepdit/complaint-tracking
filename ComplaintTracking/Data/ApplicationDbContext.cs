using ComplaintTracking.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace ComplaintTracking.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHostEnvironment _environment;

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            IHttpContextAccessor httpContextAccessor,
            IHostEnvironment environment)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            _environment = environment;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_environment.IsDevelopment())
            {
                optionsBuilder.EnableSensitiveDataLogging();
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            foreach (var entityType in builder.Model.GetEntityTypes().Where(e => typeof(IAuditable).IsAssignableFrom(e.ClrType)))
            {
                builder.Entity(entityType.ClrType)
                    .Property<DateTime?>("CreatedDate");

                builder.Entity(entityType.ClrType)
                    .Property<DateTime?>("UpdatedDate");

                builder.Entity(entityType.ClrType)
                    .Property<string>("CreatedById");

                builder.Entity(entityType.ClrType)
                    .Property<string>("UpdatedById");
            }

            builder.Entity<ActionType>().HasIndex(e => e.Name).IsUnique();
            builder.Entity<Office>().HasIndex(e => e.Name).IsUnique();
            builder.Entity<Concern>().HasIndex(e => e.Name).IsUnique();
            builder.Entity<County>().HasIndex(e => e.Name).IsUnique();
            builder.Entity<State>().HasIndex(e => e.Name).IsUnique();

            // See: https://docs.microsoft.com/en-us/aspnet/core/migration/1x-to-2x/identity-2x
            builder.Entity<ApplicationUser>()
                .HasMany(e => e.Roles)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Office>()
                .HasMany(e => e.Users)
                .WithOne(e => e.Office)
                .HasForeignKey(e => e.OfficeId);

            builder.Entity<Office>()
                .HasOne(e => e.MasterUser)
                .WithMany()
                .HasForeignKey("MasterUserId");
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            string userId = GetUserId(_httpContextAccessor.HttpContext?.User);

            foreach (var entry in ChangeTracker.Entries<IAuditable>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("CreatedById").CurrentValue = userId;
                    entry.Property("CreatedDate").CurrentValue = DateTime.Now;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property("UpdatedById").CurrentValue = userId;
                    entry.Property("UpdatedDate").CurrentValue = DateTime.Now;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        private string GetUserId(ClaimsPrincipal user)
        {
            if (user == null) return null;
            return user.FindFirst(e => e.Type == ClaimTypes.NameIdentifier)?.Value;
        }

        // Database tables
        public DbSet<ActionType> LookupActionTypes { get; set; }
        public DbSet<Office> LookupOffices { get; set; }
        public DbSet<Concern> LookupConcerns { get; set; }
        public DbSet<County> LookupCounties { get; set; }
        public DbSet<State> LookupStates { get; set; }

        public DbSet<Complaint> Complaints { get; set; }
        public DbSet<ComplaintAction> ComplaintActions { get; set; }
        public DbSet<ComplaintTransition> ComplaintTransitions { get; set; }
        public DbSet<Attachment> Attachments { get; set; }

        public DbSet<EmailLog> EmailLogs { get; set; }
    }
}
