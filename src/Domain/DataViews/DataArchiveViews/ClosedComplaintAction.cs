// ReSharper disable EntityFramework.ModelValidation.UnlimitedStringLength

namespace Cts.Domain.DataViews.DataArchiveViews;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class ClosedComplaintAction
{
    public int ComplaintId { get; set; }
    public DateOnly ActionDate { get; set; }
    public string ActionType { get; set; } = string.Empty;
    public string Comments { get; set; } = string.Empty;
    public DateTimeOffset? EnteredDate { get; set; }
    public string? EnteredBy { get; set; }
    public string Investigator { get; set; } = string.Empty;
}
