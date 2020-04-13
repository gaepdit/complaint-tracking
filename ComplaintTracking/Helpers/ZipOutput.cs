using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace ComplaintTracking
{
    public static class ZipOutput
    {
        public static async Task<MemoryStream> GetZipMemoryStreamAsync(this Dictionary<string, Task<MemoryStream>> files)
        {
            using var zipMemoryStream = new MemoryStream();
            using (var zipArchive = new ZipArchive(zipMemoryStream, ZipArchiveMode.Create, true))
            {
                foreach (var kvp in files)
                {
                    var zipEntry = zipArchive.CreateEntry(kvp.Key);
                    using var zipEntryStream = zipEntry.Open();
                    await new MemoryStream((await kvp.Value).ToArray()).CopyToAsync(zipEntryStream);
                }
            }

            return zipMemoryStream;
        }

        public static async Task<byte[]> GetZipByteArrayAsync(this Dictionary<string, Task<MemoryStream>> files)
        {
            return (await files.GetZipMemoryStreamAsync()).ToArray();
        }
    }
}
