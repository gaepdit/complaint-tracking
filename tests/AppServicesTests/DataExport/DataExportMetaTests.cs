using Cts.AppServices.DataExport;

namespace AppServicesTests.DataExport;

public class DataExportMetaTests
{
    [Test]
    public void ConstructorSetsPropertiesCorrectly()
    {
        // Arrange
        var dto = new DateTimeOffset(2024, 3, 5, 3, 50, 02, TimeSpan.Zero);
        const string expectedDateString = "2024-03-05_03-50-02";
        const string expectedFileName = "cts_export_2024-03-05_03-50-02.zip";
        var expectedExpirationDate = new DateTimeOffset(2024, 3, 5, 4, 50, 02, TimeSpan.Zero);

        // Act
        var item = new DataExportMeta(dto, cacheLifetime: 1);

        // Assert
        using var scope = new AssertionScope();
        item.FileDateString.Should().Be(expectedDateString);
        item.FileName.Should().Be(expectedFileName);
        item.FileExpirationDate.Should().Be(expectedExpirationDate);
    }
}
