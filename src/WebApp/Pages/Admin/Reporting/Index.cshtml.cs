using Cts.AppServices.Offices;
using Cts.AppServices.Permissions;
using Cts.AppServices.Reporting;
using Cts.AppServices.Staff;
using Cts.Domain.DataViews.ReportingViews;
using GaEpd.AppLibrary.ListItems;

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
    public bool ShowThresholdSelect { get; private set; }
    public bool ShowRecentActionColumns { get; private set; }
    public bool ShowStaffWithComplaintsResults { get; private set; }
    public bool ShowComplaintsResults { get; private set; }

    // Form values
    public Guid? Office { get; set; }
    public int? Threshold { get; set; }

    // Results data
    public List<StaffViewWithComplaints> StaffWithComplaints { get; private set; } = [];
    public List<ComplaintView> Complaints { get; private set; } = [];

    // Menu page
    public async Task OnGetAsync()
    {
        CanExportDataArchive = (await authorization.AuthorizeAsync(User, nameof(Policies.DataExporter))).Succeeded;
        CurrentReport = "Menu";
    }

    // === Reports ===

    public async Task OnGetDaysSinceLastActionAsync([FromQuery] Guid? office, [FromQuery] int? threshold,
        CancellationToken token)
    {
        CurrentReport = "DaysSinceLastAction";
        ShowThresholdSelect = true;
        ShowRecentActionColumns = true;
        ShowStaffWithComplaintsResults = true;
        await PopulateFormDataAsync(office, threshold, token);

        if (Office != null)
            StaffWithComplaints = await reportingService.DaysSinceLastActionAsync(Office.Value, Threshold!.Value);
    }

    public async Task OnGetComplaintsAssignedToInactiveUsersAsync([FromQuery] Guid? office, CancellationToken token)
    {
        CurrentReport = "ComplaintsAssignedToInactiveUsers";
        ShowComplaintsResults = true;

        await PopulateFormDataAsync(office, token);

        if (Office != null)
            Complaints = await reportingService.ComplaintsAssignedToInactiveUsersAsync(Office.Value);
    }

    // Select lists
    public SelectList OfficeSelectList { get; private set; } = null!;

    public SelectList AgeThresholdSelectList { get; } = new(items: new[]
    {
        new { Value = "0", Text = "All" }, new { Value = "30", Text = "30 days" },
        new { Value = "60", Text = "60 days" }, new { Value = "90", Text = "90 days" },
    }, dataValueField: "Value", dataTextField: "Text");

    private Task PopulateFormDataAsync(Guid? office, int? threshold, CancellationToken token)
    {
        Threshold = threshold ?? 30;
        return PopulateFormDataAsync(office, token);
    }

    private async Task PopulateFormDataAsync(Guid? office, CancellationToken token)
    {
        OfficeSelectList = (await officeService.GetAsListItemsAsync(token: token)).ToSelectList();
        Office = office ??
            (await staffService.GetCurrentUserAsync()).Office?.Id ??
            (await officeService.GetListAsync(token)).FirstOrDefault()?.Id;
    }

    // Reports metadata
    public Dictionary<string, string> ReportTitle { get; } = new()
    {
        { "DaysSinceLastAction", "Days Since Last Action" },
        { "ComplaintsAssignedToInactiveUsers", "Open complaints assigned to inactive users" },
    };
}
