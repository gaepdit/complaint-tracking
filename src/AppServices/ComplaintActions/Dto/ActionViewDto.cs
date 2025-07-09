using GaEpd.AppLibrary.Extensions;
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
    public string EnteredByName => new[] { EnteredByGivenName, EnteredByFamilyName }.ConcatWithSeparator();

    public string? EnteredByGivenName { get; init; }
    public string? EnteredByFamilyName { get; init; }

    [Display(Name = "Entered On")]
    public DateTimeOffset? EnteredDate { get; init; }

    // Properties: Deletion

    [Display(Name = "Deleted?")]
    public bool IsDeleted { get; init; }

    [Display(Name = "Deleted By")]
    public string DeletedByName => new[] { DeletedByGivenName, DeletedByFamilyName }.ConcatWithSeparator();

    public string? DeletedByGivenName { get; init; }
    public string? DeletedByFamilyName { get; init; }

    [Display(Name = "Date Deleted")]
    public DateTimeOffset? DeletedAt { get; init; }
}
