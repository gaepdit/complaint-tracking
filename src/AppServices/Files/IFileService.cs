namespace Cts.AppServices.Files;

public interface IFileService
{
    Task<byte[]> GetFileAsync(string path, string? location = null);
    void TryDeleteFile(string path, string? location = null);
    Task SaveFileAsync(Stream stream, string path, string? location = null);
}
