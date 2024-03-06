using System.ComponentModel.DataAnnotations;

namespace Cts.AppServices.ComplaintActions.Dto;

public record ActionUpdateDto
{
    public int ComplaintId { get; init; }

    [Required]
    [Display(Name = "Action Date")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    public DateOnly? ActionDate { get; init; }

    [Required]
    [StringLength(100)]
    public string Investigator { get; init; } = string.Empty;

    [Required]
    [Display(Name = "Action Description")]
    public Guid? ActionTypeId { get; init; }

    [Required]
    [DataType(DataType.MultilineText)]
    [StringLength(10_000)]
    public string Comments { get; init; } = string.Empty;
}
