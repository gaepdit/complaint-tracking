using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace ComplaintTracking
{
    public static class CsvOutput
    {
        public static async Task<MemoryStream> GetCsvMemoryStreamAsync<T>(this List<T> records)
        {
            using var ms = new MemoryStream();
            using var sw = new StreamWriter(ms);
            using var csv = new CsvWriter(sw, CultureInfo.InvariantCulture);

            csv.Configuration.SanitizeForInjection = true;
            csv.WriteRecords(records);

            await csv.FlushAsync();
            await sw.FlushAsync();
            await ms.FlushAsync();

            return ms;
        }

        public static async Task<byte[]> GetCsvByteArrayAsync<T>(this List<T> records)
        {
            return (await records.GetCsvMemoryStreamAsync()).ToArray();
        }

        /* Example Controller usage:

        [HttpGet]
        [Produces(CsvOutput.CsvMimeType)]
        public async Task<IActionResult> GetCsvFileDownload()
        {
            return File(await results.GetCsvByteArrayAsync(), CsvOutput.CsvMimeType, "FileName");
        }

        */
    }
}
