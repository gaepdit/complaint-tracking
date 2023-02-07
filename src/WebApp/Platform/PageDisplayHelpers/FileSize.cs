using JetBrains.Annotations;
using System.Diagnostics.CodeAnalysis;

namespace Cts.WebApp.Platform.PageDisplayHelpers;

public static class FileSize
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    private enum FileSizeUnits
    {
        [UsedImplicitly] bytes = 0,
        [UsedImplicitly] KB = 1,
        [UsedImplicitly] MB = 2,
        [UsedImplicitly] GB = 3,
        [UsedImplicitly] TB = 4,
        [UsedImplicitly] PB = 5,
        [UsedImplicitly] EB = 6,
    }

    public static string ToFileSizeString(long value)
    {
        var pow = Math.Min(Math.Floor((value > 0 ? Math.Log(value) : 0) / Math.Log(1024)),
            Enum.GetNames(typeof(FileSizeUnits)).Length); // Total number of FileSizeUnits available
        return $"{(value / Math.Pow(1024, pow)).ToString(pow == 0 ? "N0" : "N1")} {(FileSizeUnits)(int)pow}";
    }
}
