﻿using Cts.AppServices.Utilities;
using System.Globalization;

namespace AppServicesTests.Utilities;

public class DateTimeExtensionTests
{
    [TestCaseSource(nameof(_testCases))]
    public void ReturnsCorrectly(DateTime input, string output)
    {
        var result = input.RoundToNearestQuarterHour();
        result.ToString(CultureInfo.InvariantCulture).Should().Be(output);
    }

    private static object[] _testCases =
    {
        new object[] { new DateTime(2000, 01, 01, 12, 00, 00), "01/01/2000 12:00:00" },
        new object[] { new DateTime(2000, 01, 01, 12, 00, 01), "01/01/2000 12:00:00" },
        new object[] { new DateTime(2000, 01, 01, 12, 01, 00), "01/01/2000 12:00:00" },
        new object[] { new DateTime(2000, 01, 01, 12, 07, 59), "01/01/2000 12:00:00" },
        new object[] { new DateTime(2000, 01, 01, 12, 08, 00), "01/01/2000 12:15:00" },
        new object[] { new DateTime(2000, 01, 01, 12, 59, 00), "01/01/2000 13:00:00" },
    };
}
