using System.Text;

namespace Cts.WebApp.Platform.ErrorLogging;

public static class ShortId
{
    private static readonly Random Random = new();
    private const string Pool = "ABCDEFGHKMNPQRSTUVWXYZ2345689";

    public static string GetShortId(int length = 5)
    {
        var poolLength = Pool.Length;
        var output = new StringBuilder();
        for (var i = 0; i < length; i++) output.Append(Pool[Random.Next(0, poolLength)]);
        return output.ToString();
    }
}
