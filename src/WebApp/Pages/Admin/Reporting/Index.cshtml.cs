using Cts.AppServices.Complaints.QueryDto;
using Cts.AppServices.Offices;
using Cts.AppServices.Permissions;
using Cts.AppServices.Permissions.Helpers;
using Cts.AppServices.Reporting;
using Cts.AppServices.Staff;
using Cts.Domain.DataViews.ReportingViews;
using GaEpd.AppLibrary.ListItems;
using System.ComponentModel.DataAnnotations;

namespace Cts.WebApp.Pages.Admin.Reporting;

[Authorize(Policy = nameof(Policies.StaffUser))]
public class ReportingIndexModel(
    IReportingService reportingService,
    IOfficeService officeService,
    IStaffService staffService,
    IAuthorizationService authorization) : PageModel
{
    // Display properties
    public string CurrentReport { get; private set; } = string.Empty;
    public bool CanExportDataArchive { get; private set; }

    // Form display properties
    public bool ShowForm { get; private set; } = true;
    public bool ShowThresholdSelect { get; private set; }
    public bool ShowDateRange { get; private set; }
    public bool ShowOfficeSelect { get; private set; }
    public bool ShowAdminClosed { get; private set; }

    // Table column display properties
    public bool ShowRecentAction { get; private set; }
    public bool ShowDaysToClosure { get; private set; }
    public bool ShowDaysToFollowup { get; private set; }

    // Form values
    public Guid? Office { get; set; }
    public int? Threshold { get; set; }

    [Display(Name = "Date From")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    public DateOnly? From { get; set; }

    [Display(Name = "Date To")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    public DateOnly? To { get; set; }

    [Display(Name = "Include Administratively Closed Complaints")]
    public bool IncludeAdminClosed { get; set; }

    // Results data
    public bool ShowStaffReport { get; private set; }
    public List<StaffReportView> StaffReport { get; private set; } = [];

    public bool ShowOfficeReport { get; private set; }
    public List<OfficeReportView> OfficeReport { get; private set; } = [];
    public int? OfficeReportsTotalComplaints => OfficeReport.Sum(view => view.TotalComplaintsCount);

    public double? OfficeReportsTotalAvgDaysToClosure => OfficeReportsTotalComplaints == null
        ? null
        : OfficeReport.Sum(view => view.TotalDaysToClosure) / (double)OfficeReportsTotalComplaints;

    // Results linking
    public SearchComplaintStatus? LinkStatus { get; private set; }
    public bool LinkReceivedDate { get; private set; }
    public bool LinkClosedDate { get; private set; }
    public bool LinkToActionsSearch { get; private set; }
    
    // Menu page
    public async Task OnGetAsync()
    {
        CanExportDataArchive = await authorization.Succeeded(User, Policies.DataExporter);
        CurrentReport = Menu;
    }

    // === Reports ===

    public async Task OnGetComplaintsAssignedToInactiveUsersAsync()
    {
        CurrentReport = ComplaintsAssignedToInactiveUsers;
        ShowForm = false;
        ShowStaffReport = true;
        
        LinkStatus = SearchComplaintStatus.AllOpen;
        StaffReport = await reportingService.ComplaintsAssignedToInactiveUsersAsync();
    }

    public async Task OnGetComplaintsByStaffAsync(Guid? office, DateOnly? from, DateOnly? to,
        CancellationToken token)
    {
        CurrentReport = ComplaintsByStaff;
        ShowDateRange = true;
        ShowOfficeSelect = true;
        ShowStaffReport = true;

        PopulateDateRangeForm(from, to);
        await PopulateOfficeFormAsync(office, token: token);

        LinkReceivedDate = true;
        StaffReport = await reportingService.ComplaintsByStaffAsync(Office!.Value, From!.Value, To!.Value);
    }

    public async Task OnGetDaysSinceMostRecentActionAsync(Guid? office, int? threshold, CancellationToken token)
    {
        CurrentReport = DaysSinceMostRecentAction;
        ShowThresholdSelect = true;
        ShowOfficeSelect = true;
        ShowRecentAction = true;
        ShowStaffReport = true;

        PopulateThresholdForm(threshold);
        await PopulateOfficeFormAsync(office, token: token);

        LinkStatus = SearchComplaintStatus.AllOpen;
        StaffReport = await reportingService.DaysSinceMostRecentActionAsync(Office!.Value, Threshold!.Value);
    }

    public async Task OnGetDaysToClosureByOfficeAsync(DateOnly? from, DateOnly? to, bool? includeAdminClosed,
        CancellationToken token)
    {
        CurrentReport = DaysToClosureByOffice;
        ShowDateRange = true;
        ShowAdminClosed = true;
        ShowOfficeReport = true;
        
        PopulateAdminClosedForm(includeAdminClosed);
        PopulateDateRangeForm(from, to);

        LinkStatus = IncludeAdminClosed ? SearchComplaintStatus.AllClosed : SearchComplaintStatus.Closed;
        OfficeReport = await reportingService.DaysToClosureByOfficeAsync(From!.Value, To!.Value, IncludeAdminClosed);
    }

    public async Task OnGetDaysToClosureByStaffAsync(Guid? office, DateOnly? from, DateOnly? to,
        bool? includeAdminClosed, CancellationToken token)
    {
        CurrentReport = DaysToClosureByStaff;
        ShowDateRange = true;
        ShowOfficeSelect = true;
        ShowAdminClosed = true;
        ShowDaysToClosure = true;
        ShowStaffReport = true;

        PopulateAdminClosedForm(includeAdminClosed);
        PopulateDateRangeForm(from, to);
        await PopulateOfficeFormAsync(office, token: token);

        LinkClosedDate = true;
        StaffReport = await reportingService.DaysToClosureByStaffAsync(Office!.Value, From!.Value, To!.Value,
            IncludeAdminClosed);
    }

    public async Task OnGetDaysToFollowupByStaffAsync(Guid? office, DateOnly? from, DateOnly? to,
        CancellationToken token)
    {
        CurrentReport = DaysToFollowupByStaff;
        ShowDateRange = true;
        ShowOfficeSelect = true;
        ShowDaysToFollowup = true;
        ShowStaffReport = true;
        
        PopulateDateRangeForm(from, to);
        await PopulateOfficeFormAsync(office, token: token);

        LinkToActionsSearch = true;
        StaffReport = await reportingService.DaysToFollowupByStaffAsync(Office!.Value, From!.Value, To!.Value);
    }

    // Form control data
    public SelectList OfficeSelectList { get; private set; } = null!;

    public SelectList AgeThresholdSelectList { get; } = new(items: new[]
    {
        new { Value = "0", Text = "All" }, new { Value = "30", Text = "30 days" },
        new { Value = "60", Text = "60 days" }, new { Value = "90", Text = "90 days" },
    }, dataValueField: "Value", dataTextField: "Text");

    private async Task PopulateOfficeFormAsync(Guid? office, CancellationToken token)
    {
        OfficeSelectList = (await officeService.GetAsListItemsAsync(token: token)).ToSelectList();
        Office = office ??
            (await staffService.GetCurrentUserAsync()).Office?.Id ??
            (await officeService.GetListAsync(token)).FirstOrDefault()?.Id ??
            Guid.Empty;
    }

    private void PopulateThresholdForm(int? threshold) => Threshold = threshold ?? 30;

    private void PopulateAdminClosedForm(bool? includeAdminClosed) => IncludeAdminClosed = includeAdminClosed ?? false;

    private void PopulateDateRangeForm(DateOnly? dateFrom, DateOnly? dateTo)
    {
        var now = DateTime.Now;
        var currentMonth = new DateOnly(now.Year, now.Month, day: 1);
        From = dateFrom ?? currentMonth.AddMonths(-1);
        To = dateTo ?? currentMonth.AddDays(-1);
    }

    // Reports metadata
    public const string Menu = nameof(Menu);
    public const string ComplaintsAssignedToInactiveUsers = nameof(ComplaintsAssignedToInactiveUsers);
    public const string ComplaintsByStaff = nameof(ComplaintsByStaff);
    public const string DaysSinceMostRecentAction = nameof(DaysSinceMostRecentAction);
    public const string DaysToClosureByOffice = nameof(DaysToClosureByOffice);
    public const string DaysToClosureByStaff = nameof(DaysToClosureByStaff);
    public const string DaysToFollowupByStaff = nameof(DaysToFollowupByStaff);

    public Dictionary<string, string> ReportTitle { get; } = new()
    {
        { ComplaintsAssignedToInactiveUsers, "Open Complaints Assigned To Inactive Users" },
        { ComplaintsByStaff, "All Complaints By Staff" },
        { DaysSinceMostRecentAction, "Days Since Most Recent Action" },
        { DaysToClosureByOffice, "Days To Closure By Office" },
        { DaysToClosureByStaff, "Days To Closure By Staff" },
        { DaysToFollowupByStaff, "Days To Follow-up Action By Staff" },
    };
}
