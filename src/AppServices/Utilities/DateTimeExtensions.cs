namespace Cts.AppServices.Utilities;

public static class DateTimeExtensions
{
    public static TimeOnly TimeRoundedToQuarterHour(this DateTime input)
    {
        var newMinutes = 15 * (int)Math.Round(input.Minute / 15.0);
        return new TimeOnly((input.Hour + newMinutes / 60) % 24, newMinutes % 60, 0);
    }
}
