using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace ComplaintTracking
{
    public static class ZipOutput
    {
        public static async Task<MemoryStream> GetZipMemoryStreamAsync(
            this Dictionary<string, Task<MemoryStream>> files)
        {
            var zipMemoryStream = new MemoryStream();
            using var zipArchive = new ZipArchive(zipMemoryStream, ZipArchiveMode.Create, true);
            foreach (var (key, value) in files)
            {
                var zipEntry = zipArchive.CreateEntry(key);
                await using var zipEntryStream = zipEntry.Open();
                await new MemoryStream((await value).ToArray()).CopyToAsync(zipEntryStream);
            }

            return zipMemoryStream;
        }

        public static async Task<byte[]> GetZipByteArrayAsync(this Dictionary<string, Task<MemoryStream>> files) =>
            (await files.GetZipMemoryStreamAsync()).ToArray();
    }
}
