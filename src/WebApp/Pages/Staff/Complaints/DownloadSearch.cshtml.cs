using Cts.AppServices.Attachments;
using Cts.AppServices.Complaints.QueryDto;
using Cts.AppServices.DataExport;
using Cts.AppServices.Permissions;

namespace Cts.WebApp.Pages.Staff.Complaints;

[Authorize(Policy = nameof(Policies.ActiveUser))]
public class DownloadSearchModel(ISearchResultsExportService exportService) : PageModel
{
    public ComplaintSearchDto Spec { get; private set; } = null!;
    public int ResultsCount { get; private set; }

    public async Task<IActionResult> OnGetAsync(ComplaintSearchDto? spec, CancellationToken token)
    {
        if (spec is null) return BadRequest();
        ResultsCount = await exportService.CountComplaintsAsync(spec, token: token);
        Spec = spec;
        return Page();
    }

    public async Task<IActionResult> OnGetDownloadAsync(ComplaintSearchDto? spec, CancellationToken token)
    {
        if (spec is null) return BadRequest();
        var excel = (await exportService.ExportComplaintsAsync(spec, token: token))
            .ToExcel(sheetName: "Complaint Search Results", removeLastColumn: spec.DeletedStatus == null);
        var fileDownloadName = $"complaint_search_{DateTime.Now:yyyy-MM-dd--HH-mm-ss}.xlsx";
        return File(excel, FileTypes.ExcelContentType, fileDownloadName);
    }
}
