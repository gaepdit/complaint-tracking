using Cts.Domain.DataViews.ReportingViews;
using Cts.TestData.Identity;

namespace Cts.TestData.DataViews;

internal static class StaffViewWithComplaintsData
{
    public static List<StaffViewWithComplaints> GetData() =>
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
                    Status = complaint.Status,
                    ReceivedDate = complaint.ReceivedDate.Date,
                    ComplaintCounty = complaint.ComplaintCounty,
                    SourceFacilityName = complaint.SourceFacilityName,
                    LastActionDate = complaint.EnteredDate,
                    DaysSinceLastAction = 1,
                }).ToList(),
        }).ToList();
}
