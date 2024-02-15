using System.ComponentModel.DataAnnotations;

namespace Cts.AppServices.Complaints;

public record ComplaintAssignDto(int ComplaintId)
{
    [Required]
    [Display(Name = "Assigned office")]
    public Guid? OfficeId { get; init; }

    [Display(Name = "Assigned associate")]
    public string? OwnerId { get; init; }

    public string Comment { get; init; } = string.Empty;
}
