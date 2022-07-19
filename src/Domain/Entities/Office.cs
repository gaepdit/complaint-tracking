namespace Cts.Domain.Entities;

public class Office : IAuditable
{
    public Guid Id { get; set; }

    [Required]
    [StringLength(450)]
    public string Name { get; set; } = string.Empty;

    public bool Active { get; set; } = true;

    public ApplicationUser? MasterUser { get; set; }
    public string? MasterUserId { get; set; }

    public List<ApplicationUser> Users { get; set; } = new();
}
