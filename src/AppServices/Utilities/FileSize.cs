namespace Cts.AppServices.Utilities;

public static class FileSize
{
    private static string FileSizeUnits(double power) => power switch
    {
        0 => "bytes",
        1 => "KB",
        2 => "MB",
        3 => "GB",
        4 => "TB",
        5 => "PB",
        6 => "EB",
        _ => "units",
    };

    public static string ToFileSizeString(long value)
    {
        var power = Math.Min((int)Math.Floor((value > 0 ? Math.Log(value) : 0) / Math.Log(1024)),
            7); // Total number of FileSizeUnits available
        return $"{(value / Math.Pow(1024, power)).ToString(power == 0 ? "N0" : "N1")} {FileSizeUnits(power)}";
    }
}
