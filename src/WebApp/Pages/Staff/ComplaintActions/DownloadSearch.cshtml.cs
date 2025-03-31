using Cts.AppServices.Attachments;
using Cts.AppServices.ComplaintActions.Dto;
using Cts.AppServices.DataExport;
using Cts.AppServices.Permissions;

namespace Cts.WebApp.Pages.Staff.ComplaintActions;

[Authorize(Policy = nameof(Policies.ActiveUser))]
public class DownloadSearchModel(ISearchResultsExportService exportService) : PageModel
{
    public ActionSearchDto Spec { get; private set; } = null!;
    public int ResultsCount { get; private set; }

    public async Task<IActionResult> OnGetAsync(ActionSearchDto? spec, CancellationToken token)
    {
        if (spec is null) return BadRequest();
        ResultsCount = await exportService.CountActionsAsync(spec, token: token);
        Spec = spec;
        return Page();
    }

    public async Task<IActionResult> OnGetDownloadAsync(ActionSearchDto? spec, CancellationToken token)
    {
        if (spec is null) return BadRequest();
        var excel = (await exportService.ExportActionsAsync(spec, token: token))
            .ToExcel(sheetName: "Complaint Action Search Results", removeLastColumn: spec.DeletedStatus == null);
        var fileDownloadName = $"complaint_action_search_{DateTime.Now:yyyy-MM-dd--HH-mm-ss}.xlsx";
        return File(excel, FileTypes.ExcelContentType, fileDownloadName);
    }
}
