using Cts.Domain.Complaints;
using Cts.TestData.Constants;

namespace Cts.TestData;

internal static class ComplaintData
{
    private static readonly List<Complaint> ComplaintSeedItems = new()
    {
        new Complaint(1)
        {
            Status = ComplaintStatus.Closed,
            DateReceived = DateTime.Now.Date.AddDays(-2),
            ComplaintNature = TextData.MultipleParagraphs,
            ComplaintLocation = TextData.ShortMultiline,
            ComplaintCity = TextData.Word,
            ComplaintCounty = TextData.AnotherWord,
            PrimaryConcern = ConcernData.GetConcerns.ElementAt(0),
            SecondaryConcern = ConcernData.GetConcerns.ElementAt(1),
            SourceFacilityId = TextData.ThirdWord,
            SourceFacilityName = TextData.Phrase,
            SourceContactName = TextData.Phrase,
            SourceAddress = ValueObjectData.FullAddress,
            SourcePhoneNumber = ValueObjectData.SampleNumber,
            CurrentOffice = OfficeData.GetOffices.ElementAt(0),
            ReviewComments = TextData.Phrase,
            ComplaintClosed = true,
            DateComplaintClosed = DateTime.Now.Date.AddDays(-1),
        },
        new Complaint(2)
        {
            Status = ComplaintStatus.New,
            DateReceived = DateTime.Now.Date.AddDays(-1),
            ComplaintNature = null,
            ComplaintLocation = null,
            ComplaintCity = null,
            ComplaintCounty = null,
            PrimaryConcern = ConcernData.GetConcerns.ElementAt(2),
            SecondaryConcern = null,
            SourceFacilityId = null,
            SourceFacilityName = null,
            SourceContactName = null,
            SourceAddress = null,
            SourcePhoneNumber = null,
            CurrentOffice = OfficeData.GetOffices.ElementAt(1),
            ReviewComments = null,
            ComplaintClosed = false,
            DateComplaintClosed = null,
        },
        new Complaint(3)
        {
            Status = ComplaintStatus.Closed,
            DateReceived = DateTime.Now.Date.AddDays(-20),
            ComplaintNature = "PublicSearchSpec reference",
            ComplaintLocation = TextData.Word,
            ComplaintCity = TextData.Word,
            ComplaintCounty = TextData.Word,
            PrimaryConcern = ConcernData.GetConcerns.ElementAt(2),
            SecondaryConcern = ConcernData.GetConcerns.ElementAt(3),
            SourceFacilityId = TextData.Word,
            SourceFacilityName = TextData.Word,
            SourceContactName = TextData.Word,
            SourceAddress = ValueObjectData.AlternateFullAddress,
            SourcePhoneNumber = ValueObjectData.AlternateNumber,
            CurrentOffice = OfficeData.GetOffices.ElementAt(1),
            ReviewComments = TextData.Word,
            ComplaintClosed = true,
            DateComplaintClosed = DateTime.Now.Date.AddDays(-10),
        },
    };

    private static IEnumerable<Complaint>? _complaints;

    public static IEnumerable<Complaint> GetComplaints
    {
        get
        {
            if (_complaints is not null) return _complaints;
            _complaints = ComplaintSeedItems;
            return _complaints;
        }
    }
}
