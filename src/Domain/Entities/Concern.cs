namespace Cts.Domain.Entities;

public class Concern : IAuditable
{
    public Guid Id { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = "";

    public bool Active { get; set; } = true;
}
