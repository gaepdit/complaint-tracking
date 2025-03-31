using Cts.Domain.DataViews;
using Cts.Domain.DataViews.DataArchiveViews;
using GaEpd.FileService;
using Microsoft.Extensions.Caching.Memory;

namespace Cts.AppServices.DataExport;

public sealed class DataExportService(
    IDataViewRepository dataViews,
    IFileService fileService,
    IMemoryCache cache) : IDataExportService
{
    private const string DataExportCacheKey = nameof(DataExportCacheKey);
    private const int DaysToKeepExportFiles = 7;

    public async Task<DataExportMeta> ExportArchiveAsync(int cacheLifetime, string exportFilePath,
        CancellationToken token)
    {
        if (!cache.TryGetValue(DataExportCacheKey, out DataExportMeta? exportMeta))
        {
            exportMeta = new DataExportMeta(DateTimeOffset.Now, cacheLifetime);
            cache.Set(DataExportCacheKey, exportMeta, exportMeta.FileExpirationDate);
        }

        if (await fileService.FileExistsAsync(exportMeta!.FileName, exportFilePath, token: token).ConfigureAwait(false))
            return exportMeta;

        _ = DeleteOldExportFilesAsync(exportFilePath, token: token);
        await CreateDataExportFileAsync(exportMeta, exportFilePath, token: token).ConfigureAwait(false);
        return exportMeta;
    }

    public Task<List<RecordsCount>> RecordsCountAsync(CancellationToken token) => dataViews.RecordsCountAsync(token);

    private async Task CreateDataExportFileAsync(DataExportMeta exportMeta, string exportFilePath,
        CancellationToken token)
    {
        var exportFiles = new Dictionary<string, Task<MemoryStream>>
        {
            {
                $"{nameof(OpenComplaint)}_{exportMeta.FileDateString}.csv",
                (await dataViews.OpenComplaintsAsync(token).ConfigureAwait(false)).ToCsvAsync()
            },
            {
                $"{nameof(ClosedComplaint)}_{exportMeta.FileDateString}.csv",
                (await dataViews.ClosedComplaintsAsync(token).ConfigureAwait(false)).ToCsvAsync()
            },
            {
                $"{nameof(ClosedComplaintAction)}_{exportMeta.FileDateString}.csv",
                (await dataViews.ClosedComplaintActionsAsync(token).ConfigureAwait(false)).ToCsvAsync()
            },
        };

        using var zipMemoryStream = await exportFiles.CreateZipArchive().ConfigureAwait(false);
        await fileService.SaveFileAsync(zipMemoryStream, exportMeta.FileName, exportFilePath, token: token)
            .ConfigureAwait(false);
    }

    private async Task DeleteOldExportFilesAsync(string exportFilePath, CancellationToken token)
    {
        var filesAsyncEnumerable = fileService.GetFilesAsync(exportFilePath, token: token);
        await foreach (var file in filesAsyncEnumerable.ConfigureAwait(false))
        {
            // Keep only recent files for auditing.
            if (file.CreatedOn < DateTimeOffset.UtcNow.AddDays(-DaysToKeepExportFiles))
                await fileService.DeleteFileAsync(file.FullName, token: token).ConfigureAwait(false);
        }
    }

    #region IDisposable,  IAsyncDisposable

    void IDisposable.Dispose() => dataViews.Dispose();
    async ValueTask IAsyncDisposable.DisposeAsync() => await dataViews.DisposeAsync().ConfigureAwait(false);

    #endregion
}
