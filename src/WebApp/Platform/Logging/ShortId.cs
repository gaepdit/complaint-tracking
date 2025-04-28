using System.Text;

namespace Cts.WebApp.Platform.Logging;

public static class ShortId
{
    private const string Pool = "ABCDEFGHKMNPQRSTUVWXYZ2345689";
    public static string GetShortId(int length = 5) => new(Random.Shared.GetItems<char>(Pool, length));
}
