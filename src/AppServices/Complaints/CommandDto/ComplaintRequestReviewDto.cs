using System.ComponentModel.DataAnnotations;

namespace Cts.AppServices.Complaints.CommandDto;

public record ComplaintRequestReviewDto(int ComplaintId)
{
    [Required]
    [Display(Name = "Request review from")]
    public string? ReviewerId { get; init; }

    public string? Comment { get; init; } = string.Empty;
}
