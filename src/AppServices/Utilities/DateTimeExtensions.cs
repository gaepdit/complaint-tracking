namespace Cts.AppServices.Utilities;

public static class DateTimeExtensions
{
    public static DateTime RoundToNearestQuarterHour(this DateTime input)
    {
        var newTime = RoundToNearestQuarterHour(input.TimeOfDay);
        return new DateTime(input.Year, input.Month, input.Day, newTime.Hours, newTime.Minutes, 0);
    }

    private static TimeSpan RoundToNearestQuarterHour(this TimeSpan input) =>
        RoundToNearestMinutes(input, 15);

    private static TimeSpan RoundToNearestMinutes(this TimeSpan input, int minutes)
    {
        var totalMinutes = (int)(input + new TimeSpan(0, minutes / 2, 0)).TotalMinutes;
        return new TimeSpan(0, totalMinutes - totalMinutes % minutes, 0);
    }
}
