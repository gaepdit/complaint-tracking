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
            DateReceived = DateTime.Now.AddDays(-2),
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
            DateComplaintClosed = DateTime.Now.AddDays(-1),
        },
        new Complaint(2)
        {
            Status = ComplaintStatus.New,
            DateReceived = DateTime.Now.AddDays(-1),
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
