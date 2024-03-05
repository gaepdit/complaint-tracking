// ReSharper disable EntityFramework.ModelValidation.UnlimitedStringLength

namespace Cts.Domain.DataViews.DataArchiveViews;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class ClosedComplaint
{
    public int ComplaintId { get; set; }
    public string? CallerCity { get; set; }
    public string? CallerName { get; set; }
    public string? CallerPostalCode { get; set; }
    public string? CallerRepresents { get; set; }
    public string? CallerState { get; set; }
    public string? CallerStreet { get; set; }
    public string? CallerStreet2 { get; set; }
    public string? ComplaintCity { get; set; }
    public string? ComplaintCounty { get; set; }
    public string? ComplaintDirections { get; set; }
    public string? ComplaintLocation { get; set; }
    public string? ComplaintNature { get; set; }
    public string CurrentOffice { get; set; } = string.Empty;
    public string? CurrentOwner { get; set; }
    public DateTimeOffset? ComplaintClosedDate { get; set; }
    public DateTimeOffset? CurrentOwnerAcceptedDate { get; set; }
    public DateTimeOffset? CurrentOwnerAssignedDate { get; set; }
    public DateTimeOffset EnteredDate { get; set; }
    public DateTimeOffset ReceivedDate { get; set; }
    public string? EnteredBy { get; set; }
    public string PrimaryConcern { get; set; } = string.Empty;
    public string ReceivedBy { get; set; } = string.Empty;
    public string? ReviewedBy { get; set; }
    public string? ReviewComments { get; set; }
    public string? SecondaryConcern { get; set; }
    public string? SourceCity { get; set; }
    public string? SourceContactName { get; set; }
    public string? SourceFacilityId { get; set; }
    public string? SourceFacilityName { get; set; }
    public string? SourcePostalCode { get; set; }
    public string? SourceState { get; set; }
    public string? SourceStreet { get; set; }
    public string? SourceStreet2 { get; set; }
}
