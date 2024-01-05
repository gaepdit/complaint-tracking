namespace Cts.AppServices.Files;

public class FileSystemFileService(string filesBasePath) : IFileService
{
    public async Task<byte[]> GetFileAsync(string path, string? location = null)
    {
        var savePath = location is null
            ? Path.Combine(filesBasePath, path)
            : Path.Combine(filesBasePath, location, path);
        try
        {
            return await File.ReadAllBytesAsync(savePath);
        }
        catch (Exception e) when (e is FileNotFoundException or DirectoryNotFoundException)
        {
            return Array.Empty<byte>();
        }
    }

    public void TryDeleteFile(string path, string? location = null)
    {
        var savePath = location is null
            ? Path.Combine(filesBasePath, path)
            : Path.Combine(filesBasePath, location, path);

        File.Delete(Path.Combine(filesBasePath, savePath));
    }

    public async Task SaveFileAsync(Stream stream, string path, string? location = null)
    {
        var savePath = location is null
            ? Path.Combine(filesBasePath, path)
            : Path.Combine(filesBasePath, location, path);
        Directory.CreateDirectory(savePath);
        await using var fs = new FileStream(Path.Combine(filesBasePath, savePath), FileMode.Create);
        await stream.CopyToAsync(fs);
    }
}
