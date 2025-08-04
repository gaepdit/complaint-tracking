using Cts.Domain.Entities.ComplaintTransitions;
using GaEpd.AppLibrary.Extensions;

namespace Cts.AppServices.Complaints.QueryDto;

public record ComplaintTransitionViewDto
{
    public TransitionType TransitionType { get; init; }
    public DateTimeOffset CommittedDate { get; init; }

    public string CommittedByUserName =>
        new[] { CommittedByUserGivenName, CommittedByUserFamilyName }.ConcatWithSeparator();

    public string? CommittedByUserGivenName { get; init; }
    public string? CommittedByUserFamilyName { get; init; }
    public string? TransferredToUserId { get; init; }

    public string TransferredToUserName =>
        new[] { TransferredToUserGivenName, TransferredToUserFamilyName }.ConcatWithSeparator();

    public string? TransferredToUserGivenName { get; init; }
    public string? TransferredToUserFamilyName { get; init; }
    public Guid? TransferredToOfficeId { get; init; }
    public string? TransferredToOfficeName { get; init; }
    public string? Comment { get; init; }
}
