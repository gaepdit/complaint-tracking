using Cts.AppServices.Staff;
using System.ComponentModel.DataAnnotations;

namespace Cts.AppServices.ComplaintActions;

public class ComplaintActionViewDto
{
    public DateTimeOffset ActionDate { get; init; }
    public string ActionTypeName { get; init; } = default!;

    [Display(Name = "Investigator")]
    public string? Investigator { get; init; }

    public DateTimeOffset EnteredDate { get; init; }

    [Display(Name = "Entered By")]
    public StaffViewDto EnteredBy { get; init; } = default!;

    public string? Comments { get; init; }
}
