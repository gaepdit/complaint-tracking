using Cts.Domain.DataViews.ReportingViews;
using Cts.TestData.Identity;

namespace Cts.TestData.DataViews;

internal static class DataViewsTestData
{
    public static List<StaffViewWithComplaints> GetStaffViewWithComplaintsData() =>
        UserData.GetUsers.Select(user => new StaffViewWithComplaints
        {
            Id = user.Id,
            OfficeId = user.Office?.Id ?? Guid.Empty,
            GivenName = user.GivenName,
            FamilyName = user.FamilyName,
            Complaints = ComplaintData.GetComplaints
                .Where(complaint => complaint is { IsDeleted: false, ComplaintClosed: true })
                .Select(complaint => new ComplaintView
                {
                    Id = complaint.Id,
                    ReceivedDate = complaint.ReceivedDate.Date,
                    ComplaintCounty = complaint.ComplaintCounty,
                    SourceFacilityName = complaint.SourceFacilityName,
                    Status = complaint.Status,
                    MostRecentActionDate = complaint.EnteredDate,
                    ComplaintClosedDate = complaint.ComplaintClosedDate,
                    DaysSinceMostRecentAction = DateTimeOffset.Now.Date.Subtract(complaint.EnteredDate.Date).Days,
                    EarliestActionDate = DateTimeOffset.Now.AddDays(-4),
                }).ToList(),
        }).ToList();

    public static List<ComplaintView> GetComplaintViewData() =>
        ComplaintData.GetComplaints.Select(complaint => new ComplaintView
        {
            Id = complaint.Id,
            ReceivedDate = complaint.ReceivedDate,
            ComplaintCounty = complaint.ComplaintCounty,
            SourceFacilityName = complaint.SourceFacilityName,
            Status = complaint.Status,
            MostRecentActionDate = complaint.EnteredDate,
            ComplaintClosedDate = complaint.ComplaintClosedDate,
            DaysSinceMostRecentAction = DateTimeOffset.Now.Date.Subtract(complaint.EnteredDate.Date).Days,
            EarliestActionDate = DateTimeOffset.Now.AddDays(-4),
        }).ToList();
}
