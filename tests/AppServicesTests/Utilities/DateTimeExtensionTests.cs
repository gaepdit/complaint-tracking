using Cts.AppServices.Utilities;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace AppServicesTests.Utilities;

public class DateTimeExtensionTests
{
    [TestCaseSource(nameof(_testCases))]
    public void ReturnsCorrectly(string input, string output)
    {
        DateTime.ParseExact(input, D, I)
            .TimeRoundedToQuarterHour()
            .Should().Be(TimeOnly.ParseExact(output, T, I));
    }

    private const string D = "yyyy-MM-dd HH:mm:ss";
    private const string T = "HH:mm:ss";
    private static readonly CultureInfo I = CultureInfo.InvariantCulture;

    [SuppressMessage("ReSharper", "HeapView.BoxingAllocation")]
    private static object[] _testCases =
    {
        new object[] { "2000-01-01 12:00:00", "12:00:00" },
        new object[] { "2000-01-01 12:00:01", "12:00:00" },
        new object[] { "2000-01-01 12:01:00", "12:00:00" },
        new object[] { "2000-01-01 12:07:59", "12:00:00" },
        new object[] { "2000-01-01 12:08:00", "12:15:00" },
        new object[] { "2000-01-01 12:59:00", "13:00:00" },
        new object[] { "2000-01-01 00:00:00", "00:00:00" },
        new object[] { "2000-01-01 00:01:00", "00:00:00" },
        new object[] { "2000-01-01 23:59:00", "00:00:00" },
    };
}
