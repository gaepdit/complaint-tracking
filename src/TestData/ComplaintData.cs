using Cts.Domain.Entities.Complaints;
using Cts.TestData.Constants;
using Cts.TestData.Identity;

namespace Cts.TestData;

internal static class ComplaintData
{
    private static IEnumerable<Complaint> ComplaintSeedItems => new List<Complaint>
    {
        new(1) // 0
        {
            ComplaintNature = "Closed complaint",
            Status = ComplaintStatus.Closed,
            EnteredBy = UserData.GetUsers.ElementAt(0),
            ReceivedDate = DateTimeOffset.Now.AddDays(-4),
            ReceivedBy = UserData.GetUsers.ElementAt(0),
            ComplaintLocation = TextData.ShortMultiline,
            ComplaintCity = TextData.Word,
            ComplaintCounty = TextData.AnotherWord,
            PrimaryConcern = ConcernData.GetConcerns.ElementAt(0),
            SecondaryConcern = ConcernData.GetConcerns.ElementAt(10),
            SourceFacilityIdNumber = Guid.NewGuid().ToString().Substring(9, 9).ToUpperInvariant(),
            SourceFacilityName = "Closed Complaint",
            SourceContactName = TextData.Phrase,
            SourceAddress = ValueObjectData.CompleteAddress,
            SourcePhoneNumber = ValueObjectData.SamplePhoneNumber,
            SourceSecondaryPhoneNumber = ValueObjectData.SamplePhoneNumber,
            SourceTertiaryPhoneNumber = ValueObjectData.SamplePhoneNumber,
            SourceEmail = TextData.ValidEmail,
            CurrentOffice = OfficeData.GetOffices.ElementAt(0),
            CurrentOwner = UserData.GetUsers.ElementAt(0),
            CurrentOwnerAssignedDate = DateTimeOffset.Now.AddDays(-4),
            ReviewedBy = UserData.GetUsers.ElementAt(0),
            ReviewComments = TextData.Phrase,
            ComplaintClosed = true,
            ComplaintClosedDate = DateTimeOffset.Now.AddDays(-1),
            CallerName = TextData.ShortPhrase,
            CallerRepresents = TextData.AnotherWord,
            CallerAddress = ValueObjectData.CompleteAddress,
            CallerPhoneNumber = ValueObjectData.AlternatePhoneNumber,
            CallerSecondaryPhoneNumber = ValueObjectData.SamplePhoneNumber,
            CallerTertiaryPhoneNumber = ValueObjectData.AlternatePhoneNumber,
            CallerEmail = TextData.ValidEmail,
        },
        new(2) // 1
        {
            ComplaintNature = $"New complaint entered less than an hour ago. Email: {TextData.ValidEmail} & Phone: {TextData.ValidPhoneNumber}",
            Status = ComplaintStatus.New,
            EnteredBy = UserData.GetUsers.ElementAt(1),
            EnteredDate = DateTimeOffset.Now.AddMinutes(30),
            ReceivedDate = DateTimeOffset.Now.AddDays(-1),
            ReceivedBy = UserData.GetUsers.ElementAt(1),
            PrimaryConcern = ConcernData.GetConcerns.ElementAt(2),
            SecondaryConcern = ConcernData.GetConcerns.ElementAt(3),
            CurrentOffice = OfficeData.GetOffices.ElementAt(1),
            SourceFacilityName = "new with minimal data.",
            CallerName = TextData.ShortPhrase,
            CallerRepresents = TextData.AnotherWord,
            CallerAddress = ValueObjectData.CompleteAddress,
            ComplaintLocation = $"Email: {TextData.ValidEmail} & Phone: {TextData.ValidPhoneNumber}",
        },
        new(3) // 2
        {
            ComplaintNature = "PublicSearchSpec complaint nature reference",
            Status = ComplaintStatus.Closed,
            EnteredBy = UserData.GetUsers.ElementAt(2),
            ReceivedDate = DateTimeOffset.Now.AddDays(-20),
            ReceivedBy = UserData.GetUsers.ElementAt(2),
            ComplaintLocation = TextData.Word,
            ComplaintCity = TextData.Word,
            ComplaintCounty = TextData.Word,
            PrimaryConcern = ConcernData.GetConcerns.ElementAt(2),
            SecondaryConcern = ConcernData.GetConcerns.ElementAt(13),
            SourceFacilityIdNumber = Guid.NewGuid().ToString().Substring(9, 9).ToUpperInvariant(),
            SourceFacilityName = "PublicSearchSpec facility name reference",
            SourceContactName = TextData.Word,
            SourceAddress = ValueObjectData.AlternateFullCompleteAddress,
            SourcePhoneNumber = ValueObjectData.AlternatePhoneNumber,
            CurrentOffice = OfficeData.GetOffices.ElementAt(1),
            CurrentOwner = UserData.GetUsers.ElementAt(2),
            ReviewComments = TextData.Word,
            ComplaintClosed = true,
            ComplaintClosedDate = DateTimeOffset.Now.AddDays(-10),
            CallerPhoneNumber = ValueObjectData.AlternatePhoneNumber,
        },
        new(5) // 3
        {
            ComplaintNature = "Deleted complaint",
            Status = ComplaintStatus.Closed,
            EnteredBy = UserData.GetUsers.ElementAt(0),
            ReceivedDate = DateTimeOffset.Now.AddDays(-2),
            ReceivedBy = UserData.GetUsers.ElementAt(0),
            ComplaintLocation = TextData.ShortMultiline,
            ComplaintCity = TextData.Word,
            ComplaintCounty = TextData.AnotherWord,
            PrimaryConcern = ConcernData.GetConcerns.ElementAt(0),
            SecondaryConcern = ConcernData.GetConcerns.ElementAt(14),
            SourceFacilityIdNumber = Guid.NewGuid().ToString().Substring(9, 9).ToUpperInvariant(),
            SourceFacilityName = "Deleted Complaint",
            SourceContactName = TextData.Phrase,
            SourceAddress = ValueObjectData.CompleteAddress,
            SourcePhoneNumber = ValueObjectData.SamplePhoneNumber,
            CurrentOffice = OfficeData.GetOffices.ElementAt(0),
            ReviewComments = TextData.Phrase,
            ComplaintClosed = true,
            ComplaintClosedDate = DateTimeOffset.Now.AddDays(-1),
            DeleteComments = TextData.Paragraph,
        },
        new(6) // 4
        {
            ComplaintNature = "Open complaint assigned to user.",
            Status = ComplaintStatus.New,
            EnteredBy = UserData.GetUsers.ElementAt(1),
            ReceivedDate = DateTimeOffset.Now.AddDays(-1),
            ReceivedBy = UserData.GetUsers.ElementAt(1),
            PrimaryConcern = ConcernData.GetConcerns.ElementAt(2),
            CurrentOffice = OfficeData.GetOffices.ElementAt(1),
            CurrentOwner = UserData.GetUsers.ElementAt(1),
            CurrentOwnerAssignedDate = DateTimeOffset.Now.AddDays(-4),
        },
        new(7) // 5
        {
            ComplaintNature = "Complaint accepted by user and under investigation.",
            Status = ComplaintStatus.UnderInvestigation,
            EnteredBy = UserData.GetUsers.ElementAt(1),
            ReceivedDate = DateTimeOffset.Now.AddDays(-1),
            ReceivedBy = UserData.GetUsers.ElementAt(1),
            PrimaryConcern = ConcernData.GetConcerns.ElementAt(2),
            CurrentOffice = OfficeData.GetOffices.ElementAt(1),
            CurrentOwner = UserData.GetUsers.ElementAt(1),
            CurrentOwnerAssignedDate = DateTimeOffset.Now.AddDays(-4),
            CurrentOwnerAcceptedDate = DateTimeOffset.Now.AddDays(-3),
        },
        new(8) // 6
        {
            ComplaintNature = "New complaint entered more than an hour ago.",
            Status = ComplaintStatus.New,
            EnteredBy = UserData.GetUsers.ElementAt(1),
            EnteredDate = DateTimeOffset.Now.AddHours(-2),
            ReceivedDate = DateTimeOffset.Now.AddDays(-1),
            ReceivedBy = UserData.GetUsers.ElementAt(1),
            PrimaryConcern = ConcernData.GetConcerns.ElementAt(2),
            CurrentOffice = OfficeData.GetOffices.ElementAt(1),
        },
        new(4) // 7
        {
            ComplaintNature = "Ready to accept",
            Status = ComplaintStatus.New,
            EnteredBy = UserData.GetUsers.ElementAt(0),
            EnteredDate = DateTimeOffset.Now.AddDays(-1),
            ReceivedDate = DateTimeOffset.Now.AddDays(-1),
            ReceivedBy = UserData.GetUsers.ElementAt(0),
            PrimaryConcern = ConcernData.GetConcerns.ElementAt(2),
            CurrentOffice = OfficeData.GetOffices.ElementAt(1),
            CurrentOwner = UserData.GetUsers.ElementAt(1),
            CurrentOwnerAssignedDate = DateTimeOffset.Now.AddDays(-1),
            SourceFacilityName = "Assigned to staff user (General).",
        },
    };

    private static IEnumerable<Complaint>? _complaints;

    public static IEnumerable<Complaint> GetComplaints
    {
        get
        {
            if (_complaints is not null) return _complaints;

            _complaints = ComplaintSeedItems.ToList();
            _complaints.ElementAt(3).SetDeleted("00000000-0000-0000-0000-000000000001");

            foreach (var complaint in _complaints)
            {
                complaint.Actions.AddRange(ComplaintActionData.GetComplaintActions
                    .Where(e => e.Complaint.Id == complaint.Id));
            }

            return _complaints;
        }
    }

    public static void ClearData() => _complaints = null;
}
