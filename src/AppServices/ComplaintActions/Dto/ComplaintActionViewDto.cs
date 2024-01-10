using Cts.AppServices.Staff.Dto;
using System.ComponentModel.DataAnnotations;

namespace Cts.AppServices.ComplaintActions;

public record ComplaintActionViewDto
{
    public Guid Id { get; [UsedImplicitly] init; }

    [Display(Name = "Action Type")]
    public string ActionTypeName { get; init; } = default!;

    [Display(Name = "Action Date")]
    public DateOnly ActionDate { get; init; }

    [Display(Name = "Investigator")]
    public string? Investigator { get; init; }

    public string? Comments { get; init; }

    [Display(Name = "Entered By")]
    public StaffViewDto? EnteredBy { get; [UsedImplicitly] init; } = default!;

    [Display(Name = "Entered On")]
    public DateTimeOffset? EnteredDate { get; init; }
}
