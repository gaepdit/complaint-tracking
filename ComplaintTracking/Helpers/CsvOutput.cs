using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;

namespace ComplaintTracking
{
    public static class CsvOutput
    {
        public static async Task<MemoryStream> GetCsvMemoryStreamAsync<T>(this List<T> records)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture) {SanitizeForInjection = true};
            var ms = new MemoryStream();
            await using var sw = new StreamWriter(ms);
            await using var csv = new CsvWriter(sw, config);

            await csv.WriteRecordsAsync(records);

            await csv.FlushAsync();
            await sw.FlushAsync();
            await ms.FlushAsync();

            return ms;
        }

        public static async Task<byte[]> GetCsvByteArrayAsync<T>(this List<T> records) =>
            (await records.GetCsvMemoryStreamAsync()).ToArray();
    }
}
