namespace Cts.AppServices.Utilities;

public static class DateTimeFormats
{
    // Display strings
    public const string LongDateTimeFormat = "MMMM\u00a0d, yyyy h:mm\u00a0tt";
    public const string LongDateFormat = "MMMM\u00a0d, yyyy";
    public const string ShortDateFormat = "d\u2011MMM\u2011yyyy";
    public const string ShortDateTimeFormat = "d\u2011MMM\u2011yyyy h:mm\u00a0tt";
    public const string ShortDateTimeNoBreakFormat = "d\u2011MMM\u2011yyyy\u00a0h:mm\u00a0tt";

    // Format strings   
    public const string DateOnlyInput = "{0:O}";
    public const string RouteValue = "yyyy-MM-dd";
}
