using Microsoft.EntityFrameworkCore;

namespace ComplaintTracking.Entities.ActionTypes;

[Index(nameof(Name), IsUnique = true)]
public class ActionType : AuditedEntity<Guid>
{
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    public bool Active { get; set; }
}
