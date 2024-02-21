using System.ComponentModel.DataAnnotations;

namespace Cts.AppServices.Complaints.CommandDto;

public record ComplaintAssignmentDto(int ComplaintId)
{
    [Required]
    [Display(Name = "Assigned office")]
    public Guid? OfficeId { get; init; }

    [Display(Name = "Assigned associate")]
    public string? OwnerId { get; init; }

    public string? Comment { get; init; }
}
