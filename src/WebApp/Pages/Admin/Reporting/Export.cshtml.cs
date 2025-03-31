using Cts.AppServices.Attachments;
using Cts.AppServices.DataExport;
using Cts.AppServices.Permissions;
using Cts.Domain.DataViews.DataArchiveViews;
using Cts.WebApp.Platform.Constants;
using GaEpd.FileService;

namespace Cts.WebApp.Pages.Admin.Reporting;

[Authorize(Policy = nameof(Policies.DataExporter))]
public class ExportModel(IDataExportService dataExportService, IFileService fileService)
    : PageModel
{
    public bool Downloading { get; private set; }
    public List<RecordsCount> RecordsCounts { get; private set; } = [];

    public async Task OnGetAsync(CancellationToken token = default) =>
        RecordsCounts.AddRange(await dataExportService.RecordsCountAsync(token));

    public IActionResult OnGetDownloading() => RedirectToPage("Export");

    public void OnPostDownloading() => Downloading = true;

    public async Task<IActionResult> OnGetArchiveAsync(CancellationToken token = default)
    {
        var exportMeta = await dataExportService.ExportArchiveAsync(cacheLifetime: GlobalConstants.ExportLifespan,
            exportFilePath: GlobalConstants.ExportFolder, token: token);
        var response = await fileService.TryGetFileAsync(exportMeta.FileName, GlobalConstants.ExportFolder, token: token);

        return response.Success
            ? File(response.Value, FileTypes.ZipContentType, exportMeta.FileName)
            : NotFound("An error occurred creating the data export. Please try again later. " +
                "If you continue to receive this message, please contact EPD IT support.");
    }
}
