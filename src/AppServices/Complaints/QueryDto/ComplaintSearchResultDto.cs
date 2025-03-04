using Cts.AppServices.Staff.Dto;
using Cts.Domain.Entities.Complaints;

namespace Cts.AppServices.Complaints.QueryDto;

public record ComplaintSearchResultDto
{
    public int Id { get; init; }
    public DateTimeOffset ReceivedDate { get; init; }
    public ComplaintStatus Status { get; init; }
    public bool ComplaintClosed { get; init; }
    public DateTimeOffset? ComplaintClosedDate { get; init; }
    public string? SourceFacilityName { get; init; }
    public string? SourceFacilityIdNumber { get; init; }
    public string? ComplaintCity { get; init; }
    public string? ComplaintCounty { get; init; }
    public string? SourceAddressCityState { get; init; }
    public string? PrimaryConcernName { get; init; }
    public string? SecondaryConcernName { get; init; }
    public string? CurrentOfficeName { get; init; }
    public StaffViewDto? CurrentOwner { get; init; }
    public string? AssignedOwnerName => CurrentOwner?.SortableFullName;
    public bool IsDeleted { get; init; }
}
