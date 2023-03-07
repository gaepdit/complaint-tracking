using Cts.Domain.Complaints;
using Cts.TestData.Constants;
using Cts.TestData.Identity;

namespace Cts.TestData;

internal static class ComplaintData
{
    private static List<Complaint> ComplaintSeedItems => new()
    {
        new Complaint(1)
        {
            Status = ComplaintStatus.Closed,
            EnteredBy = UserData.GetUsers.ElementAt(0),
            DateReceived = DateTimeOffset.Now.AddDays(-4),
            ReceivedBy = UserData.GetUsers.ElementAt(0),
            ComplaintNature = TextData.MultipleParagraphs,
            ComplaintLocation = TextData.ShortMultiline,
            ComplaintCity = TextData.Word,
            ComplaintCounty = TextData.AnotherWord,
            PrimaryConcern = ConcernData.GetConcerns.ElementAt(0),
            SecondaryConcern = ConcernData.GetConcerns.ElementAt(1),
            SourceFacilityId = Guid.NewGuid().ToString().Substring(9, 9).ToUpperInvariant(),
            SourceFacilityName = TextData.Phrase,
            SourceContactName = TextData.Phrase,
            SourceAddress = ValueObjectData.FullCompleteAddress(),
            SourcePhoneNumber = ValueObjectData.SamplePhoneNumber(),
            SourceSecondaryPhoneNumber = ValueObjectData.SamplePhoneNumber(),
            SourceTertiaryPhoneNumber = ValueObjectData.SamplePhoneNumber(),
            SourceEmail = TextData.EmailAddress,
            CurrentOffice = OfficeData.GetOffices.ElementAt(0),
            CurrentOwner = UserData.GetUsers.ElementAt(1),
            DateCurrentOwnerAssigned = DateTimeOffset.Now.AddDays(-4),
            ReviewedBy = UserData.GetUsers.ElementAt(0),
            ReviewComments = TextData.Phrase,
            ComplaintClosed = true,
            DateComplaintClosed = DateTimeOffset.Now.AddDays(-1),
            CallerName = TextData.ShortPhrase,
            CallerRepresents = TextData.AnotherWord,
            CallerAddress = ValueObjectData.FullCompleteAddress(),
            CallerPhoneNumber = ValueObjectData.AlternatePhoneNumber(),
            CallerSecondaryPhoneNumber = ValueObjectData.SamplePhoneNumber(),
            CallerTertiaryPhoneNumber = ValueObjectData.AlternatePhoneNumber(),
            CallerEmail= TextData.EmailAddress,
        },
        new Complaint(2)
        {
            Status = ComplaintStatus.New,
            EnteredBy = UserData.GetUsers.ElementAt(1),
            DateReceived = DateTimeOffset.Now.AddDays(-1),
            ReceivedBy = UserData.GetUsers.ElementAt(1),
            PrimaryConcern = ConcernData.GetConcerns.ElementAt(2),
            CurrentOffice = OfficeData.GetOffices.ElementAt(1),
        },
        new Complaint(3)
        {
            Status = ComplaintStatus.Closed,
            EnteredBy = UserData.GetUsers.ElementAt(2),
            DateReceived = DateTimeOffset.Now.AddDays(-20),
            ReceivedBy = UserData.GetUsers.ElementAt(2),
            ComplaintNature = "PublicSearchSpec complaint nature reference",
            ComplaintLocation = TextData.Word,
            ComplaintCity = TextData.Word,
            ComplaintCounty = TextData.Word,
            PrimaryConcern = ConcernData.GetConcerns.ElementAt(2),
            SecondaryConcern = ConcernData.GetConcerns.ElementAt(3),
            SourceFacilityId = Guid.NewGuid().ToString().Substring(9, 9).ToUpperInvariant(),
            SourceFacilityName = "PublicSearchSpec facility name reference",
            SourceContactName = TextData.Word,
            SourceAddress = ValueObjectData.AlternateFullCompleteAddress(),
            SourcePhoneNumber = ValueObjectData.AlternatePhoneNumber(),
            CurrentOffice = OfficeData.GetOffices.ElementAt(1),
            CurrentOwner = UserData.GetUsers.ElementAt(2),
            ReviewComments = TextData.Word,
            ComplaintClosed = true,
            DateComplaintClosed = DateTimeOffset.Now.AddDays(-10),
            CallerPhoneNumber = ValueObjectData.AlternatePhoneNumber(),
        },
        new Complaint(5)
        {
            ComplaintNature = "Deleted Complaint",
            Status = ComplaintStatus.Closed,
            EnteredBy = UserData.GetUsers.ElementAt(0),
            DateReceived = DateTimeOffset.Now.AddDays(-2),
            ReceivedBy = UserData.GetUsers.ElementAt(0),
            ComplaintLocation = TextData.ShortMultiline,
            ComplaintCity = TextData.Word,
            ComplaintCounty = TextData.AnotherWord,
            PrimaryConcern = ConcernData.GetConcerns.ElementAt(0),
            SecondaryConcern = ConcernData.GetConcerns.ElementAt(1),
            SourceFacilityId = Guid.NewGuid().ToString().Substring(9, 9).ToUpperInvariant(),
            SourceFacilityName = TextData.Phrase,
            SourceContactName = TextData.Phrase,
            SourceAddress = ValueObjectData.FullCompleteAddress(),
            SourcePhoneNumber = ValueObjectData.SamplePhoneNumber(),
            CurrentOffice = OfficeData.GetOffices.ElementAt(0),
            ReviewComments = TextData.Phrase,
            ComplaintClosed = true,
            DateComplaintClosed = DateTimeOffset.Now.AddDays(-1),
        },
    };

    private static IEnumerable<Complaint>? _complaints;

    public static IEnumerable<Complaint> GetComplaints
    {
        get
        {
            if (_complaints is not null) return _complaints;

            _complaints = ComplaintSeedItems;
            _complaints.ElementAt(3).SetDeleted("00000000-0000-0000-0000-000000000001");

            foreach (var complaint in _complaints)
            {
                complaint.ComplaintActions = ComplaintActionData.GetComplaintActions
                    .Where(e => e.Complaint.Id == complaint.Id).ToList();
            }

            return _complaints;
        }
    }

    public static void ClearData() => _complaints = null;
}
