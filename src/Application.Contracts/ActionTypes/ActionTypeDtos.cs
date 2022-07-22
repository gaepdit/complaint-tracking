namespace ComplaintTracking.ActionTypes;

public class ViewActionTypeDto : AuditedEntityDto<Guid>
{
    public string Name { get; set; } = string.Empty;
    public bool Active { get; set; }
}

public class CreateUpdateActionTypeDto : AuditedEntityDto<Guid>
{
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    public bool Active { get; set; } = true;
}
