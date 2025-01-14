using Cts.AppServices.Staff.Dto;

namespace Cts.AppServices.ComplaintActions.Dto;

public record ActionSearchResultDto
{
    public int ComplaintId { get; init; }
    public string ActionTypeName { get; init; } = string.Empty;
    public DateOnly ActionDate { get; init; }
    public string? Investigator { get; init; }
    public StaffViewDto? EnteredBy { get; init; }
    public string? EnteredByName => EnteredBy?.SortableFullName;
    public string? EnteredByOffice => EnteredBy?.Office?.Name;
    public string? Comments { get; init; }
    public bool IsDeleted { get; init; }
    public bool ComplaintIsDeleted { get; init; }
}
