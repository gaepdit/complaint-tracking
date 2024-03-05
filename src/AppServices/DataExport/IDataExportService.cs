using Cts.Domain.DataViews.DataArchiveViews;

namespace Cts.AppServices.DataExport;

public interface IDataExportService : IDisposable, IAsyncDisposable
{
    Task<DataExportMeta> ExportArchiveAsync(int cacheLifetime, string exportFilePath, CancellationToken token);
    Task<List<RecordsCount>> RecordsCountAsync(CancellationToken token);
}

public record DataExportMeta
{
    public DataExportMeta(DateTimeOffset exportDate, int cacheLifetime)
    {
        FileDateString = $"{exportDate:yyyy-MM-dd_HH-mm-ss}";
        FileName = $"cts_export_{FileDateString}.zip";
        FileExpirationDate = exportDate.AddHours(cacheLifetime);
    }

    public string FileDateString { get; }
    public string FileName { get; }
    public DateTimeOffset FileExpirationDate { get; }
}
