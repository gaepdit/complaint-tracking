namespace Cts.AppServices.Files;

public class FileSystemFileService : IFileService
{
    private readonly string _filesBasePath;
    public FileSystemFileService(string filesBasePath) => _filesBasePath = filesBasePath;

    public async Task<byte[]> GetFileAsync(string path, string? location = null)
    {
        var savePath = location is null
            ? Path.Combine(_filesBasePath, path)
            : Path.Combine(_filesBasePath, location, path);
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
            ? Path.Combine(_filesBasePath, path)
            : Path.Combine(_filesBasePath, location, path);

        File.Delete(Path.Combine(_filesBasePath, savePath));
    }

    public async Task SaveFileAsync(Stream stream, string path, string? location = null)
    {
        var savePath = location is null
            ? Path.Combine(_filesBasePath, path)
            : Path.Combine(_filesBasePath, location, path);
        Directory.CreateDirectory(savePath);
        await using var fs = new FileStream(Path.Combine(_filesBasePath, savePath), FileMode.Create);
        await stream.CopyToAsync(fs);
    }
}
