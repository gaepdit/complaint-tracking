using Cts.AppServices.Staff;
using Cts.Domain.Complaints;

namespace Cts.AppServices.Complaints.Dto;

public class ComplaintSearchResultDto
{
    public int Id { get; init; }
    public DateTimeOffset ReceivedDate { get; init; }
    public ComplaintStatus Status { get; init; }
    public bool ComplaintClosed { get; init; }
    public DateTimeOffset? ComplaintClosedDate { get; init; }
    public string? SourceFacilityName { get; init; }
    public string? ComplaintLocation { get; init; }
    public string? SourceFacilityIdNumber { get; init; }
    public string? ComplaintCounty { get; init; }
    public string? PrimaryConcernName { get; init; }
    public string? SecondaryConcernName { get; init; }
    public string? CurrentOfficeName { get; init; }
    public StaffViewDto? CurrentOwner { get; set; }
    public string? AssignedOwnerName => CurrentOwner?.SortableFullName;
    public bool IsDeleted { get; set; }

}
