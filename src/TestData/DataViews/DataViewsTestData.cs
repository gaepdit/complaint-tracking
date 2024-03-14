using Cts.Domain.DataViews.ReportingViews;
using Cts.TestData.Identity;

namespace Cts.TestData.DataViews;

internal static class DataViewsTestData
{
    public static List<StaffReportView> GetStaffReportData() =>
        UserData.GetUsers.Select(user => new StaffReportView
        {
            Id = user.Id,
            OfficeId = user.Office?.Id ?? Guid.Empty,
            GivenName = user.GivenName,
            FamilyName = user.FamilyName,
            Complaints = ComplaintData.GetComplaints
                .Where(complaint => complaint is { IsDeleted: false, ComplaintClosed: true })
                .Select(complaint => new ComplaintReportView
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

    public static List<OfficeReportView> GetOfficeReportData() =>
        OfficeData.GetOffices.Select(office => new OfficeReportView
        {
            OfficeId = office.Id,
            OfficeName = office.Name,
            TotalComplaintsCount = 55,
            AverageDaysToClosure = 12.3,
            TotalDaysToClosure = 677,
        }).ToList();
}
