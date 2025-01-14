using System.ComponentModel.DataAnnotations;

namespace Cts.AppServices.ComplaintActions.Dto;

public record ActionPublicViewDto
{
    public Guid Id { get; [UsedImplicitly] init; }

    [Display(Name = "Action Type")]
    public string ActionTypeName { get; init; } = string.Empty;

    [Display(Name = "Action Date")]
    public DateOnly ActionDate { get; init; }

    [Display(Name = "Investigator")]
    public string? Investigator { get; init; }

    public string? Comments { get; init; }
}
