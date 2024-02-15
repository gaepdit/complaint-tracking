namespace Cts.AppServices.Complaints;

// Used for approving/closing and reopening complaints.
public record ComplaintClosureDto(int ComplaintId)
{
    public string Comment { get; init; } = string.Empty;
}
