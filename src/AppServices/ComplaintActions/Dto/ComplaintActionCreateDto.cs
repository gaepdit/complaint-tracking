using System.ComponentModel.DataAnnotations;

namespace Cts.AppServices.ComplaintActions;

public record ComplaintActionCreateDto(int ComplaintId)
{
    [Required]
    [Display(Name = "Action Description")]
    public Guid? ActionTypeId { get; init; }

    [Required]
    [Display(Name = "Action Date")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    public DateOnly? ActionDate { get; init; } = DateOnly.FromDateTime(DateTime.Today);

    [Required]
    [StringLength(100)]
    public string Investigator { get; init; } = string.Empty;

    [Required]
    public string Comments { get; init; } = string.Empty;
}
