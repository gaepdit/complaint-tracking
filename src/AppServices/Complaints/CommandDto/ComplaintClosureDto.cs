using System.ComponentModel.DataAnnotations;

namespace Cts.AppServices.Complaints.CommandDto;

// Used for approving/closing, reopening, deleting, and restoring complaints.
public record ComplaintClosureDto(int ComplaintId)
{
    [DataType(DataType.MultilineText)]
    [StringLength(7000)]
    public string? Comment { get; init; } = string.Empty;
}
