using Cts.AppServices.Staff.Dto;
using Cts.Domain.Entities.ActionTypes;

namespace Cts.AppServices.ComplaintActions;

public record ComplaintActionSearchResultDto
{
    public int ComplaintId { get; init; }
    public string ActionTypeName { get; init; } = default!;
    public DateOnly ActionDate { get; init; }
    public string? Investigator { get; init; }
    public StaffViewDto? EnteredBy { get; init; }
    public string? EnteredByName => EnteredBy?.SortableFullName;
    public string? EnteredByOffice => EnteredBy?.Office?.Name;
    public string? Comments { get; init; }
    public bool IsDeleted { get; init; }
    public bool ComplaintIsDeleted { get; init; }
}
