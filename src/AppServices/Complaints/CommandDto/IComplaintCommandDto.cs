using Cts.Domain.ValueObjects;

namespace Cts.AppServices.Complaints.CommandDto;

public interface IComplaintCommandDto
{
    // Meta-data
    DateOnly ReceivedDate { get; }
    TimeOnly ReceivedTime { get; }
    string? ReceivedById { get; }

    // Caller
    string? CallerName { get; }
    string? CallerRepresents { get; }
    string? CallerEmail { get; }
    PhoneNumber? CallerPhoneNumber { get; }
    PhoneNumber? CallerSecondaryPhoneNumber { get; }
    PhoneNumber? CallerTertiaryPhoneNumber { get; }
    IncompleteAddress CallerAddress { get; }

    // Complaint details
    string? ComplaintNature { get; }
    string? ComplaintLocation { get; }
    string? ComplaintDirections { get; }
    string? ComplaintCity { get; }
    string? ComplaintCounty { get; }
    Guid PrimaryConcernId { get; }
    Guid? SecondaryConcernId { get; }

    // Source
    string? SourceFacilityName { get; }
    string? SourceFacilityIdNumber { get; }

    // Source Contact
    string? SourceContactName { get; }
    string? SourceEmail { get; }
    PhoneNumber? SourcePhoneNumber { get; }
    PhoneNumber? SourceSecondaryPhoneNumber { get; }
    PhoneNumber? SourceTertiaryPhoneNumber { get; }

    // Source Address
    IncompleteAddress SourceAddress { get; }
}
