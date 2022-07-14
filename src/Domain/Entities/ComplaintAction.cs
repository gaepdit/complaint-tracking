namespace Cts.Domain.Entities;

public class ComplaintAction : IAuditable
{
    public Guid Id { get; set; }

    public Complaint Complaint { get; set; } = null!;
    public int ComplaintId { get; set; }

    public DateTime ActionDate { get; set; }

    public ActionType ActionType { get; set; } = null!;
    public Guid ActionTypeId { get; set; }

    [StringLength(100)]
    public string? Investigator { get; set; }

    public DateTime? DateEntered { get; set; }

    public ApplicationUser EnteredBy { get; set; } = null!;
    public string EnteredById { get; set; } = "";

    public string? Comments { get; set; }

    public bool Deleted { get; set; }

    public DateTime? DateDeleted { get; set; }

    public ApplicationUser? DeletedBy { get; set; }
    public string? DeletedById { get; set; }
}
