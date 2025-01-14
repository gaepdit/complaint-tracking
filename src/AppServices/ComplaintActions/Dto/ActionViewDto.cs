using Cts.AppServices.Staff.Dto;
using System.ComponentModel.DataAnnotations;

namespace Cts.AppServices.ComplaintActions.Dto;

public record ActionViewDto
{
    public Guid Id { get; [UsedImplicitly] init; }
    public int ComplaintId { get; [UsedImplicitly] init; }

    [Display(Name = "Action Type")]
    public string ActionTypeName { get; init; } = string.Empty;

    [Display(Name = "Action Date")]
    public DateOnly ActionDate { get; init; }

    [Display(Name = "Investigator")]
    public string? Investigator { get; init; }

    public string? Comments { get; init; }

    [Display(Name = "Entered By")]
    public StaffViewDto? EnteredBy { get; [UsedImplicitly] init; }

    [Display(Name = "Entered On")]
    public DateTimeOffset? EnteredDate { get; init; }

    // Properties: Deletion

    [Display(Name = "Deleted?")]
    public bool IsDeleted { get; init; }

    public string? DeletedById { get; init; }

    [Display(Name = "Deleted By")]
    public StaffViewDto? DeletedBy { get; set; }

    [Display(Name = "Date Deleted")]
    public DateTimeOffset? DeletedAt { get; init; }
}
