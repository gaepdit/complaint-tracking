using System.IO.Compression;

namespace ComplaintTracking
{
    public static class ZipOutput
    {
        public static async Task<MemoryStream> GetZipMemoryStreamAsync(this Dictionary<string, Task<MemoryStream>> files)
        {
            var zipMemoryStream = new MemoryStream();
            using (var zipArchive = new ZipArchive(zipMemoryStream, ZipArchiveMode.Create, leaveOpen: true))
            {

                foreach (var (key, value) in files)
                {
                    var zipEntry = zipArchive.CreateEntry(key);
                    await using var zipEntryStream = zipEntry.Open();
                    await new MemoryStream((await value).ToArray()).CopyToAsync(zipEntryStream);
                }
            } // `zipArchive` must be disposed before resetting position and returning stream. Otherwise, the Position might move again.

            zipMemoryStream.Position = 0;
            return zipMemoryStream;
        }
    }
}
