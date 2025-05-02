namespace Cts.Domain;

public static class AppConstants
{
    // For length validation
    public const int MinimumNameLength = 2;
    public const int MaximumNameLength = 50;

    // Time-based permissions
    public static int RecentReporterDuration => 1; // hours

    // SQL Server supported date range
    public static readonly DateOnly SqlServerMinDate = new(1753, 1, 1);
    public static readonly DateOnly SqlServerMaxDate = new(9999, 12, 31);
}
