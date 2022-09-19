using Cts.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Cts.Infrastructure.Contexts;

public class CtsDbContext : IdentityDbContext<ApplicationUser>
{
    public CtsDbContext(DbContextOptions<CtsDbContext> options) : base(options) { }

    public DbSet<ActionType> ActionTypes => Set<ActionType>();
    public DbSet<Attachment> Attachments => Set<Attachment>();
    public DbSet<Complaint> Complaints => Set<Complaint>();
    public DbSet<ComplaintAction> ComplaintActions => Set<ComplaintAction>();
    public DbSet<ComplaintTransition> ComplaintTransitions => Set<ComplaintTransition>();
    public DbSet<Concern> Concerns => Set<Concern>();
    public DbSet<Office> Offices => Set<Office>();
    public DbSet<EmailLog> EmailLogs => Set<EmailLog>();
}
