﻿using Cts.AppServices.Utilities;

namespace AppServicesTests.Utilities;

public class FileSizeTests
{
    [Test]
    [TestCase(0, "0 bytes")]
    [TestCase(1, "1 bytes")]
    [TestCase(10, "10 bytes")]
    [TestCase(1024, "1.0 KB")]
    [TestCase(2048, "2.0 KB")]
    [TestCase(99999999, "95.4 MB")]
    public void ReturnsCorrectString(long value, string expected)
    {
        FileSize.ToFileSizeString(value).Should().Be(expected);
    }
}
