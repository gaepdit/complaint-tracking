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
    public Task<List<ComplaintReportView>> ComplaintsAssignedToInactiveUsersAsync(Guid officeId) =>
        context.Complaints.AsNoTracking()
            .Where(c => !c.ComplaintClosed && !c.IsDeleted && c.CurrentOffice.Id == officeId &&
                c.CurrentOwner != null && !c.CurrentOwner.Active)
            .OrderByDescending(complaint => complaint.ReceivedDate)
            .Select(complaint => new ComplaintReportView
            {
                Id = complaint.Id,
                ReceivedDate = complaint.ReceivedDate,
                Status = complaint.Status,
                ComplaintCounty = complaint.ComplaintCounty,
                SourceFacilityName = complaint.SourceFacilityName,
            }).ToListAsync();

    public async Task<List<StaffReportView>> DaysSinceMostRecentActionAsync(Guid officeId, int threshold) =>
        await QueryStaffReportAsync(ReportingQueries.DaysSinceMostRecentAction,
            new { officeId, threshold }).ConfigureAwait(false);

    public async Task<List<OfficeReportView>> DaysToClosureByOfficeAsync(DateOnly dateFrom, DateOnly dateTo,
        bool includeAdminClosed)
    {
        var midnight = new TimeOnly(0, 0, 0); // Only needed until Dapper supports DateOnly.
        var parameters = new
        {
            dateFrom = dateFrom.ToDateTime(midnight),
            dateTo = dateTo.ToDateTime(midnight),
            includeAdminClosed,
        };
        using var db = dbConnection.Create();
        return (await db.QueryAsync<OfficeReportView>(
            sql: ReportingQueries.DaysToClosureByOffice, param: parameters,
            commandType: CommandType.Text).ConfigureAwait(false)).ToList();
    }

    public async Task<List<StaffReportView>> DaysToClosureByStaffAsync(Guid officeId, DateOnly dateFrom,
        DateOnly dateTo, bool includeAdminClosed)
    {
        var midnight = new TimeOnly(0, 0, 0); // Only needed until Dapper supports DateOnly.
        return await QueryStaffReportAsync(ReportingQueries.DaysToClosureByStaff,
            new
            {
                officeId,
                dateFrom = dateFrom.ToDateTime(midnight),
                dateTo = dateTo.ToDateTime(midnight),
                includeAdminClosed,
            }).ConfigureAwait(false);
    }

    public async Task<List<StaffReportView>> DaysToFollowupByStaffAsync(Guid officeId, DateOnly dateFrom,
        DateOnly dateTo)
    {
        var midnight = new TimeOnly(0, 0, 0); // Only needed until Dapper supports DateOnly.
        return await QueryStaffReportAsync(ReportingQueries.DaysToFollowupByStaff,
            new
            {
                officeId,
                dateFrom = dateFrom.ToDateTime(midnight),
                dateTo = dateTo.ToDateTime(midnight),
            }).ConfigureAwait(false);
    }

    private async Task<List<StaffReportView>> QueryStaffReportAsync(string sql, object parameters)
    {
        var staffDictionary = new Dictionary<string, StaffReportView>();

        using var db = dbConnection.Create();
        _ = await db.QueryAsync<StaffReportView, ComplaintReportView, StaffReportView>(
            sql: sql, map: MapRecord, param: parameters,
            commandType: CommandType.Text).ConfigureAwait(false);

        return staffDictionary.Values.ToList();

        StaffReportView MapRecord(StaffReportView staff, ComplaintReportView complaint)
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
