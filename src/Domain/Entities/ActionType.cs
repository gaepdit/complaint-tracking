namespace Cts.Domain.Entities;

public class ActionType : IAuditable
{
    public Guid Id { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    public bool Active { get; set; } = true;
}
