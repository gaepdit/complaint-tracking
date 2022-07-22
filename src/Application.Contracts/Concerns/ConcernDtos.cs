namespace ComplaintTracking.Concerns;

public class ViewConcernDto : AuditedEntityDto<Guid>
{
    public string Name { get; set; } = string.Empty;
    public bool Active { get; set; }
}

public class CreateUpdateConcernDto : AuditedEntityDto<Guid>
{
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    public bool Active { get; set; } = true;
}
