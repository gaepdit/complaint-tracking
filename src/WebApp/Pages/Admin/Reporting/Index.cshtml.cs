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
    public bool ViewReportsMenu { get; private set; }
    public bool CanExportDataArchive { get; private set; }
    public string CurrentReport { get; private set; } = string.Empty;
    public string CurrentReportName { get; private set; } = string.Empty;

    // Form values

    public Guid? Office { get; set; }
    public int? Threshold { get; set; }

    // Menu page

    public async Task OnGetAsync()
    {
        CanExportDataArchive = (await authorization.AuthorizeAsync(User, nameof(Policies.DataExporter))).Succeeded;
        ViewReportsMenu = true;
    }

    // Report
    // "Days Since Last Action"
    public List<StaffViewWithComplaints> StaffWithComplaints { get; private set; } = [];

    public async Task OnGetDaysSinceLastActionAsync([FromQuery] Guid? office, [FromQuery] int? threshold,
        CancellationToken token)
    {
        CurrentReportName = "Days Since Last Action";
        CurrentReport = nameof(OnGetDaysSinceLastActionAsync);

        OfficeSelectList = (await officeService.GetAsListItemsAsync(token: token)).ToSelectList();
        Threshold = threshold ?? 30;
        Office = office ??
            (await staffService.GetCurrentUserAsync()).Office?.Id ??
            (await officeService.GetListAsync(token)).FirstOrDefault()?.Id ??
            Guid.Empty;

        if (Office != Guid.Empty)
            StaffWithComplaints = await reportingService.DaysSinceLastActionAsync(Office.Value, Threshold.Value, token);
    }

    // Select lists
    public SelectList OfficeSelectList { get; private set; } = null!;

    public SelectList AgeThresholdSelectList { get; } = new(items: new[]
    {
        new { Value = "0", Text = "All" }, new { Value = "30", Text = "30 days" },
        new { Value = "60", Text = "60 days" }, new { Value = "90", Text = "90 days" },
    }, dataValueField: "Value", dataTextField: "Text");
}
