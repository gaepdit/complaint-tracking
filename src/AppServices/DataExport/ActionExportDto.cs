using ClosedXML.Attributes;
using Cts.Domain.Entities.ComplaintActions;

namespace Cts.AppServices.DataExport;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public record ActionExportDto
{
    public ActionExportDto(ComplaintAction action)
    {
        ComplaintId = action.ComplaintId;
        ActionDate = action.ActionDate;
        ActionType = action.ActionType.Name;
        Investigator = action.Investigator;
        ActionComments = action.Comments;
        Deleted = action.IsDeleted || action.Complaint.IsDeleted ? "Deleted" : "No";
    }

    [XLColumn(Header = "Complaint ID")]
    public int ComplaintId { get; init; }

    [XLColumn(Header = "Action Date")]
    public DateOnly? ActionDate { get; init; }

    [XLColumn(Header = "Action Type")]
    public string? ActionType { get; init; }

    [XLColumn(Header = "Investigator")]
    public string? Investigator { get; init; }

    [XLColumn(Header = "Action Comments")]
    public string? ActionComments { get; init; }

    [XLColumn(Header = "Deleted?")]
    public string Deleted { get; init; }
}
