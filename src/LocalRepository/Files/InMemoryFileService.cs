using Cts.AppServices.Files;
using Cts.TestData;

namespace Cts.LocalRepository.Files;

public class InMemoryFileService : IFileService
{
    private ICollection<AttachmentFile> Items { get; } = AttachmentData.GetAttachmentFiles;

    public Task<byte[]> GetFileAsync(string path, string? location = null)
    {
        var attachmentFile = Items
            .SingleOrDefault(e => e.FileName == path && e.Location == location);
        return attachmentFile is null || string.IsNullOrEmpty(attachmentFile.Base64EncodedFile)
            ? Task.FromResult(Array.Empty<byte>())
            : Task.FromResult(Convert.FromBase64String(attachmentFile.Base64EncodedFile));
    }

    public void TryDeleteFile(string path, string? location = null)
    {
        var item = Items.SingleOrDefault(e => e.FileName == path && e.Location == location);
        if (item is not null) Items.Remove(item);
    }

    public async Task SaveFileAsync(Stream stream, string path, string? location = null)
    {
        using var ms = new MemoryStream();
        await stream.CopyToAsync(ms);
        var attachmentFile = new AttachmentFile(path, location, Convert.ToBase64String(ms.ToArray()));

        Items.Add(attachmentFile);
    }
}
