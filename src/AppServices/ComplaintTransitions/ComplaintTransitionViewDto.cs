using Cts.AppServices.Offices;
using Cts.AppServices.Staff;
using Cts.Domain.Entities.ComplaintTransitions;

namespace Cts.AppServices.ComplaintTransitions;

public class ComplaintTransitionViewDto
{
    public TransitionType TransitionType { get; set; }
    public DateTimeOffset TransferredDate { get; set; }
    public StaffViewDto? CommittedByUser { get; set; }
    public StaffViewDto? TransferredFromUser { get; set; }
    public OfficeDisplayViewDto? TransferredFromOffice { get; set; }
    public StaffViewDto? TransferredToUser { get; set; }
    public OfficeDisplayViewDto? TransferredToOffice { get; set; }
    public string? Comment { get; set; }
}
