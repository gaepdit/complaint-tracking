﻿using Cts.Domain.DataViews.ReportingViews;

namespace Cts.AppServices.Reporting;

public interface IReportingService : IDisposable, IAsyncDisposable
{
    Task<List<StaffViewWithComplaints>> DaysSinceLastActionAsync(Guid officeId, int threshold);
    Task<List<ComplaintView>> ComplaintsAssignedToInactiveUsersAsync(Guid officeId);
}