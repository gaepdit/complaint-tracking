using System.ComponentModel.DataAnnotations;

namespace Cts.AppServices.ComplaintActions;

public record ComplaintActionUpdateDto
{
    public int ComplaintId { get; init; }

    [Required]
    [Display(Name = "Action Date")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    public DateOnly ActionDate { get; init; } = DateOnly.FromDateTime(DateTime.Today);

    [Required]
    [Display(Name = "Action Description")]
    public Guid? ActionTypeId { get; init; }

    [Required]
    [StringLength(100)]
    public string? Investigator { get; init; }

    public string? Comments { get; init; }
}
