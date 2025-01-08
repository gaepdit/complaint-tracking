using ClosedXML.Excel;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.IO.Compression;
using System.Text;

namespace Cts.AppServices.DataExport;

public static class DataExportUtilities
{
    /// <summary>
    /// Creates an Excel spreadsheet from <see cref="IEnumerable{T}"/> records. 
    /// </summary>
    /// <param name="records">The records to add to the spreadsheet.</param>
    /// <param name="sheetName">A name for the worksheet.</param>
    /// <param name="removeLastColumn">A flag indicating whether to remove the final column in the table
    /// (the column showing deletion status).</param>
    /// <typeparam name="T">The type of the records being inserted.</typeparam>
    /// <returns>A <see cref="MemoryStream"/> containing an Excel spreadsheet with a worksheet named
    /// <paramref name="sheetName"/> containing the data in <paramref name="records"/> as a table.</returns>
    public static MemoryStream ToExcel<T>(this IEnumerable<T> records, string sheetName,
        bool removeLastColumn)
    {
        using var xlWorkbook = new XLWorkbook();

        var xlWorksheet = xlWorkbook.AddWorksheet(sheetName);
        var xlTable = xlWorksheet.Cell(row: 1, column: 1).InsertTable(records);
        if (removeLastColumn) xlTable.Column(xlTable.Columns().Count()).Delete();
        xlTable.Cells().Style
            .Alignment.SetWrapText(true)
            .Alignment.SetVertical(XLAlignmentVerticalValues.Top);
        xlWorksheet.Columns().AdjustToContents(minWidth: 8d, maxWidth: 80d);

        var memoryStream = new MemoryStream();
        xlWorkbook.SaveAs(memoryStream);
        memoryStream.Position = 0;

        return memoryStream;
    }

    /// <summary>
    /// Creates an CSV formatted output from <see cref="IEnumerable{T}"/> records. 
    /// </summary>
    /// <param name="records">The records to add to the CSV output.</param>
    /// <typeparam name="T">The type of the records being inserted.</typeparam>
    /// <returns>A <see cref="MemoryStream"/> containing the <paramref name="records"/> formatted as CSV.</returns>
    public static async Task<MemoryStream> ToCsvAsync<T>(this IEnumerable<T> records)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture) { InjectionOptions = InjectionOptions.Escape };
        var memoryStream = new MemoryStream();

        var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8);
        var csvWriter = new CsvWriter(streamWriter, config);
        await using var streamWriterAsyncDisposable = streamWriter.ConfigureAwait(false);
        await using var csvWriterAsyncDisposable = csvWriter.ConfigureAwait(false);

        await csvWriter.WriteRecordsAsync(records).ConfigureAwait(false);

        await csvWriter.FlushAsync().ConfigureAwait(false);
        await streamWriter.FlushAsync().ConfigureAwait(false);
        await memoryStream.FlushAsync().ConfigureAwait(false);

        return memoryStream;
    }

    /// <summary>
    /// Creates a <see cref="ZipArchive"/> of the <paramref name="files"/>.
    /// </summary>
    /// <param name="files">A <see cref="Dictionary{TKey,TValue}"/> with filenames as the keys and
    /// <see cref="MemoryStream"/> files as the values.</param>
    /// <returns>A <see cref="MemoryStream"/> containing the files in a ZIP archive.</returns>
    public static async Task<MemoryStream> CreateZipArchive(this Dictionary<string, Task<MemoryStream>> files)
    {
        var zipMemoryStream = new MemoryStream();
        using (var zipArchive = new ZipArchive(zipMemoryStream, ZipArchiveMode.Create, leaveOpen: true))
        {
            foreach (var (key, value) in files)
            {
                var zipEntry = zipArchive.CreateEntry(key);
                var zipEntryStream = zipEntry.Open();
                await using var zipEntryStreamAsyncDisposable = zipEntryStream.ConfigureAwait(false);
                await new MemoryStream((await value.ConfigureAwait(false)).ToArray()).CopyToAsync(zipEntryStream)
                    .ConfigureAwait(false);
            }
        } // `zipArchive` must be disposed before resetting position and returning stream.
        // Otherwise, the Position might move again.

        zipMemoryStream.Position = 0;
        return zipMemoryStream;
    }
}
