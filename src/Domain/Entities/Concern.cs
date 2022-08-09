using Cts.Domain.Entities.BaseEntities;

namespace Cts.Domain.Entities;

public class Concern : AuditableEntity
{
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    public bool Active { get; set; } = true;
}
