using Microsoft.EntityFrameworkCore;

namespace ComplaintTracking.Entities.Concerns;

[Index(nameof(Name), IsUnique = true)]
public class Concern : AuditedEntity<Guid>
{
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    public bool Active { get; set; } 
}
