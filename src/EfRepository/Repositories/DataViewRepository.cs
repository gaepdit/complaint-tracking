using Cts.Domain.DataViews;
using Cts.Domain.DataViews.DataArchiveViews;
using Cts.Domain.DataViews.ReportingViews;
using Cts.EfRepository.DbConnection;
using Cts.EfRepository.DbObjects.Reporting;
using Dapper;
using System.Data;

namespace Cts.EfRepository.Repositories;

public sealed class DataViewRepository(AppDbContext context, IDbConnectionFactory dbConnection) : IDataViewRepository
{
    // Data archive export
    public Task<List<OpenComplaint>> OpenComplaintsAsync(CancellationToken token) =>
        context.OpenComplaintsView.ToListAsync(cancellationToken: token);

    public Task<List<ClosedComplaint>> ClosedComplaintsAsync(CancellationToken token) =>
        context.ClosedComplaintsView.ToListAsync(cancellationToken: token);

    public Task<List<ClosedComplaintAction>> ClosedComplaintActionsAsync(CancellationToken token) =>
        context.ClosedComplaintActionsView.ToListAsync(cancellationToken: token);

    public Task<List<RecordsCount>> RecordsCountAsync(CancellationToken token) =>
        context.RecordsCountView.OrderBy(recordsCount => recordsCount.Order).ToListAsync(cancellationToken: token);

    // Reporting
    public async Task<List<StaffViewWithComplaints>> DaysSinceMostRecentActionAsync(Guid officeId, int threshold) =>
        await QueryStaffViewWithComplaintsList(ReportingQueries.DaysSinceMostRecentAction,
            new { officeId, threshold }).ConfigureAwait(false);

    public Task<List<ComplaintView>> ComplaintsAssignedToInactiveUsersAsync(Guid officeId) =>
        context.Complaints.AsNoTracking()
            .Where(c => !c.ComplaintClosed && !c.IsDeleted && c.CurrentOffice.Id == officeId &&
                c.CurrentOwner != null && !c.CurrentOwner.Active)
            .OrderByDescending(complaint => complaint.ReceivedDate)
            .Select(complaint => new ComplaintView
            {
                Id = complaint.Id,
                ReceivedDate = complaint.ReceivedDate,
                Status = complaint.Status,
                ComplaintCounty = complaint.ComplaintCounty,
                SourceFacilityName = complaint.SourceFacilityName,
            }).ToListAsync();


    public async Task<List<StaffViewWithComplaints>> DaysToClosureByStaffAsync(Guid officeId, DateOnly dateFrom,
        DateOnly dateTo, bool includeAdminClosed)
    {
        var midnight = new TimeOnly(0, 0, 0); // Only needed until Dapper supports DateOnly.
        return await QueryStaffViewWithComplaintsList(ReportingQueries.DaysToClosureByStaff,
            new
            {
                officeId,
                dateFrom = dateFrom.ToDateTime(midnight),
                dateTo = dateTo.ToDateTime(midnight),
                includeAdminClosed,
            }).ConfigureAwait(false);
    }

    public async Task<List<StaffViewWithComplaints>> DaysToFollowupByStaffAsync(Guid officeId, DateOnly dateFrom,
        DateOnly dateTo)
    {
        var midnight = new TimeOnly(0, 0, 0); // Only needed until Dapper supports DateOnly.
        return await QueryStaffViewWithComplaintsList(ReportingQueries.DaysToFollowupByStaff,
            new
            {
                officeId,
                dateFrom = dateFrom.ToDateTime(midnight),
                dateTo = dateTo.ToDateTime(midnight),
            }).ConfigureAwait(false);
    }

    private async Task<List<StaffViewWithComplaints>> QueryStaffViewWithComplaintsList(string sql, object parameters)
    {
        var staffDictionary = new Dictionary<string, StaffViewWithComplaints>();

        using var db = dbConnection.Create();
        _ = await db.QueryAsync<StaffViewWithComplaints, ComplaintView, StaffViewWithComplaints>(
            sql: sql, map: MapRecord, param: parameters,
            commandType: CommandType.Text).ConfigureAwait(false);

        return staffDictionary.Values.ToList();

        StaffViewWithComplaints MapRecord(StaffViewWithComplaints staff, ComplaintView complaint)
        {
            if (!staffDictionary.TryGetValue(staff.Id, out var staffView))
            {
                staffView = staff;
                staffDictionary.Add(staffView.Id, staffView);
            }

            staffView.Complaints.Add(complaint);
            return staffView;
        }
    }

    #region IDisposable,  IAsyncDisposable

    void IDisposable.Dispose() => context.Dispose();
    async ValueTask IAsyncDisposable.DisposeAsync() => await context.DisposeAsync().ConfigureAwait(false);

    #endregion
}
