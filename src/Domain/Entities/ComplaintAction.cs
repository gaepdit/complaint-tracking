using Cts.Domain.Entities.BaseEntities;
using Cts.Domain.Users;

namespace Cts.Domain.Entities;

public class ComplaintAction : SoftDeleteEntity
{
    public Complaint Complaint { get; set; } = null!;
    public int ComplaintId { get; set; }

    public DateTime ActionDate { get; set; }

    public ActionType ActionType { get; set; } = null!;
    public Guid ActionTypeId { get; set; }

    [StringLength(100)]
    public string? Investigator { get; set; }

    public DateTime? DateEntered { get; set; }

    public ApplicationUser EnteredBy { get; set; } = null!;

    public string? Comments { get; set; }
}
