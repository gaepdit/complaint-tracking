using ClosedXML.Excel;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace ComplaintTracking
{
    public static class DataExport
    {
        public static async Task<MemoryStream> GetCsvMemoryStreamAsync<T>(this IEnumerable<T> records)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture) { InjectionOptions = InjectionOptions.Escape };
            var ms = new MemoryStream();
            await using var sw = new StreamWriter(ms);
            await using var csv = new CsvWriter(sw, config);

            await csv.WriteRecordsAsync(records);

            await csv.FlushAsync();
            await sw.FlushAsync();
            await ms.FlushAsync();

            return ms;
        }

        public static MemoryStream ExportExcelAsStream<T>(this IEnumerable<T> records)
        {
            var wb = new XLWorkbook();
            var ws = wb.AddWorksheet("CTS Search Results");

            ws.Cell(1, 1).InsertTable(records);
            ws.Columns().AdjustToContents(8.0d, 80.0d);

            var ms = new MemoryStream();
            wb.SaveAs(ms);

            ms.Position = 0;
            return ms;
        }
    }
}
