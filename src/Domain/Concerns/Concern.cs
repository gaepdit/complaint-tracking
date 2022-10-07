namespace Cts.Domain.Concerns;

public class Concern : AuditableEntity
{
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    public bool Active { get; set; } = true;
}
