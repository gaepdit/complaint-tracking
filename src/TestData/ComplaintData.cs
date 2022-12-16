using Cts.Domain.Complaints;

namespace Cts.TestData;

internal static class ComplaintData
{
    private static readonly List<Complaint> ComplaintSeedItems = new()
    {
        new Complaint(1)
        {
            ComplaintNature = "Test complaint",
        },
    };

    private static ICollection<Complaint>? _complaints;

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
