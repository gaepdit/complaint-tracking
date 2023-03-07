using Cts.AppServices.Offices;
using Cts.AppServices.Staff;
using Cts.Domain.ComplaintTransitions;

namespace Cts.AppServices.ComplaintTransitions;

public class ComplaintTransitionViewDto
{
    public StaffViewDto? TransferredByUser { get; set; }
    public StaffViewDto? TransferredFromUser { get; set; }
    public OfficeDisplayViewDto? TransferredFromOffice { get; set; }
    public StaffViewDto? TransferredToUser { get; set; }
    public OfficeDisplayViewDto? TransferredToOffice { get; set; }
    public DateTimeOffset DateTransferred { get; set; }
    public DateTimeOffset? DateAccepted { get; set; }
    public TransitionType TransitionType { get; set; }
    public string? Comment { get; set; }
}
