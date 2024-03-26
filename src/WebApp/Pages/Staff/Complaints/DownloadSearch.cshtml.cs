using Cts.AppServices.Attachments;
using Cts.AppServices.Complaints.QueryDto;
using Cts.AppServices.DataExport;
using Cts.AppServices.Permissions;
using Cts.AppServices.Permissions.Helpers;

namespace Cts.WebApp.Pages.Staff.Complaints;

[Authorize(Policy = nameof(Policies.ActiveUser))]
public class DownloadSearchModel(
    ISearchResultsExportService searchResultsExportService,
    IAuthorizationService authorization) : PageModel
{
    public ComplaintSearchDto Spec { get; private set; } = default!;
    public int ResultsCount { get; private set; }

    public async Task<IActionResult> OnGetAsync(ComplaintSearchDto? spec, CancellationToken token)
    {
        if (spec is null) return BadRequest();
        spec.TrimAll();
        if (!await authorization.Succeeded(User, Policies.DivisionManager))
            spec.DeletedStatus = null;
        ResultsCount = await searchResultsExportService.CountAsync(spec, token);
        Spec = spec;
        return Page();
    }

    public async Task<IActionResult> OnGetDownloadAsync(ComplaintSearchDto? spec, CancellationToken token)
    {
        if (spec is null) return BadRequest();
        spec.TrimAll();
        if (!await authorization.Succeeded(User, Policies.DivisionManager))
            spec.DeletedStatus = null;
        var excel = (await searchResultsExportService.ExportSearchResultsAsync(spec, token))
            .ToExcel(sheetName: "CTS Search Results", deleteLastColumn: spec.DeletedStatus == null);
        var fileDownloadName = $"cts_search_{DateTime.Now:yyyy-MM-dd--HH-mm-ss}.xlsx";
        return File(excel, FileTypes.ExcelContentType, fileDownloadName: fileDownloadName);
    }
}
