using GaEpd.AppLibrary.Extensions;

namespace Cts.Domain.DataViews.ReportingViews;

public class StaffViewWithComplaints
{
    public string Id { get; init; } = string.Empty;
    public Guid OfficeId { get; init; }
    public string GivenName { get; init; } = string.Empty;
    public string FamilyName { get; init; } = string.Empty;
    public string SortableFullName => new[] { FamilyName, GivenName }.ConcatWithSeparator(", ");
    public List<ComplaintView> Complaints { get; init; } = [];
}
