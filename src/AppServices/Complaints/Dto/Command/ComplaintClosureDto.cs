namespace Cts.AppServices.Complaints;

// Used for approving/closing, reopening, deleting, and restoring complaints.
public record ComplaintClosureDto(int ComplaintId)
{
    public string? Comment { get; init; } = string.Empty;
}
