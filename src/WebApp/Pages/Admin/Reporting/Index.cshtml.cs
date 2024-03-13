using Cts.AppServices.Offices;
using Cts.AppServices.Permissions;
using Cts.AppServices.Reporting;
using Cts.AppServices.Staff;
using Cts.Domain.DataViews.ReportingViews;
using GaEpd.AppLibrary.ListItems;
using System.ComponentModel.DataAnnotations;

namespace Cts.WebApp.Pages.Admin.Reporting;

[Authorize(Policy = nameof(Policies.ActiveUser))]
public class IndexModel(
    IReportingService reportingService,
    IOfficeService officeService,
    IStaffService staffService,
    IAuthorizationService authorization) : PageModel
{
    // Display properties
    public string CurrentReport { get; private set; } = string.Empty;
    public bool CanExportDataArchive { get; private set; }

    // Form display properties
    public bool ShowThresholdSelect { get; private set; }
    public bool ShowDateRange { get; private set; }
    public bool ShowAdminClosed { get; private set; }

    // Results display properties
    public bool ShowStaffList { get; private set; }
    public bool ShowComplaintsList { get; private set; }
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
    public List<StaffViewWithComplaints> StaffList { get; private set; } = [];
    public List<ComplaintView> ComplaintsList { get; private set; } = [];

    // Menu page
    public async Task OnGetAsync()
    {
        CanExportDataArchive = (await authorization.AuthorizeAsync(User, nameof(Policies.DataExporter))).Succeeded;
        CurrentReport = Menu;
    }

    // === Reports ===

    public async Task OnGetComplaintsAssignedToInactiveUsersAsync([FromQuery] Guid? office, CancellationToken token)
    {
        CurrentReport = ComplaintsAssignedToInactiveUsers;
        ShowComplaintsList = true;

        await PopulateFormDataAsync(office, token);

        ComplaintsList = await reportingService.ComplaintsAssignedToInactiveUsersAsync(Office!.Value);
    }

    public async Task OnGetDaysSinceMostRecentActionAsync([FromQuery] Guid? office, [FromQuery] int? threshold,
        CancellationToken token)
    {
        CurrentReport = DaysSinceMostRecentAction;
        ShowThresholdSelect = true;
        ShowRecentAction = true;
        ShowStaffList = true;

        await PopulateFormDataAsync(office, threshold, token);

        StaffList = await reportingService.DaysSinceMostRecentActionAsync(Office!.Value, Threshold!.Value);
    }

    public async Task OnGetDaysToClosureByStaffAsync([FromQuery] Guid? office, DateOnly? from, DateOnly? to,
        bool? includeAdminClosed, CancellationToken token)
    {
        CurrentReport = DaysToClosureByStaff;
        ShowDateRange = true;
        ShowAdminClosed = true;
        ShowDaysToClosure = true;
        ShowStaffList = true;
        IncludeAdminClosed = includeAdminClosed ?? false;
        await PopulateFormDataAsync(office, from, to, token);

        StaffList = await reportingService.DaysToClosureByStaffAsync(Office!.Value, From!.Value, To!.Value,
            IncludeAdminClosed);
    }

    public async Task OnGetDaysToFollowupByStaffAsync([FromQuery] Guid? office, DateOnly? from, DateOnly? to, CancellationToken token)
    {
        CurrentReport = DaysToFollowupByStaff;
        ShowDateRange = true;
        ShowDaysToFollowup = true;
        ShowStaffList = true;
        await PopulateFormDataAsync(office, from, to, token);

        StaffList = await reportingService.DaysToFollowupByStaffAsync(Office!.Value, From!.Value, To!.Value);
    }

    // Form control data
    public SelectList OfficeSelectList { get; private set; } = null!;

    public SelectList AgeThresholdSelectList { get; } = new(items: new[]
    {
        new { Value = "0", Text = "All" }, new { Value = "30", Text = "30 days" },
        new { Value = "60", Text = "60 days" }, new { Value = "90", Text = "90 days" },
    }, dataValueField: "Value", dataTextField: "Text");

    private async Task PopulateFormDataAsync(Guid? office, CancellationToken token)
    {
        OfficeSelectList = (await officeService.GetAsListItemsAsync(token: token)).ToSelectList();
        Office = office ??
            (await staffService.GetCurrentUserAsync()).Office?.Id ??
            (await officeService.GetListAsync(token)).FirstOrDefault()?.Id ??
            Guid.Empty;
    }

    private Task PopulateFormDataAsync(Guid? office, int? threshold, CancellationToken token)
    {
        Threshold = threshold ?? 30;
        return PopulateFormDataAsync(office, token);
    }

    private Task PopulateFormDataAsync(Guid? office, DateOnly? dateFrom, DateOnly? dateTo, CancellationToken token)
    {
        var now = DateTime.Now;
        var currentMonth = new DateOnly(now.Year, now.Month, day: 1);
        From = dateFrom ?? currentMonth.AddMonths(-1);
        To = dateTo ?? currentMonth.AddDays(-1);
        return PopulateFormDataAsync(office, token);
    }

    // Reports metadata
    public const string Menu = nameof(Menu);
    public const string ComplaintsAssignedToInactiveUsers = nameof(ComplaintsAssignedToInactiveUsers);
    public const string ComplaintsByCounty = nameof(ComplaintsByCounty);
    public const string ComplaintsByStaff = nameof(ComplaintsByStaff);
    public const string DaysSinceMostRecentAction = nameof(DaysSinceMostRecentAction);
    public const string DaysToClosureByOffice = nameof(DaysToClosureByOffice);
    public const string DaysToClosureByStaff = nameof(DaysToClosureByStaff);
    public const string DaysToFollowupByStaff = nameof(DaysToFollowupByStaff);
    public const string UnconfirmedUserAccounts = nameof(UnconfirmedUserAccounts);
    public const string UsersAssignedToInactiveOffices = nameof(UsersAssignedToInactiveOffices);

    public Dictionary<string, string> ReportTitle { get; } = new()
    {
        { ComplaintsAssignedToInactiveUsers, "Open complaints assigned to inactive users" },
        { ComplaintsByCounty, "" },
        { ComplaintsByStaff, "" },
        { DaysSinceMostRecentAction, "Days Since Most Recent Action" },
        { DaysToClosureByOffice, "" },
        { DaysToClosureByStaff, "Days To Closure By Staff" },
        { DaysToFollowupByStaff, "Days To Follow-up By Staff" },
        { UnconfirmedUserAccounts, "" },
        { UsersAssignedToInactiveOffices, "" },
    };
}
