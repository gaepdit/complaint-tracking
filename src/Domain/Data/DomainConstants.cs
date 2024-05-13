namespace Cts.Domain.Data;

public static class DomainConstants
{
    // Date of final data migration from old DNR Oracle CTS application 
    // into new EPD application: September 30, 2017, EDT
    public static readonly DateTimeOffset OracleMigrationDate = new(year: 2017, month: 9, day: 30, hour: 0, minute: 0,
        second: 0, offset: new TimeSpan(hours: -4, minutes: 0, seconds: 0));
}
