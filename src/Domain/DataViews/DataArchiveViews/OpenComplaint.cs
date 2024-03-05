// ReSharper disable EntityFramework.ModelValidation.UnlimitedStringLength

namespace Cts.Domain.DataViews.DataArchiveViews;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class OpenComplaint
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
    public string PrimaryConcern { get; set; } = string.Empty;
    public string? SecondaryConcern { get; set; }
    public DateTimeOffset ReceivedDate { get; set; }
    public string ReceivedBy { get; set; } = string.Empty;
}
