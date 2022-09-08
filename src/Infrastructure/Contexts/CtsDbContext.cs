using Cts.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Cts.Infrastructure.Contexts;

public class CtsDbContext : IdentityDbContext<ApplicationUser>
{
    public CtsDbContext(DbContextOptions<CtsDbContext> options) : base(options) { }

    public DbSet<ActionType> ActionTypes { get; set; } = null!;
    public DbSet<Attachment> Attachments { get; set; } = null!;
    public DbSet<Complaint> Complaints { get; set; } = null!;
    public DbSet<ComplaintAction> ComplaintActions { get; set; } = null!;
    public DbSet<ComplaintTransition> ComplaintTransitions { get; set; } = null!;
    public DbSet<Concern> Concerns { get; set; } = null!;
    public DbSet<Office> Offices { get; set; } = null!;
    public DbSet<EmailLog> EmailLogs { get; set; } = null!;
}
