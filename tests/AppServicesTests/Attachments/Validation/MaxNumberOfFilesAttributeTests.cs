using Cts.AppServices.Attachments.ValidationAttributes;
using Cts.TestData.Constants;
using Microsoft.AspNetCore.Http;

namespace AppServicesTests.Attachments.Validation;

[TestFixture]
[TestOf(typeof(MaxNumberOfFilesAttribute))]
public class MaxNumberOfFilesAttributeTests
{
    private static List<IFormFile> FormFiles =>
    [
        new FormFile(AppServiceHelpers.TestStream, 0, 1, TextData.ValidName, TextData.ValidPdfFileName),
        new FormFile(AppServiceHelpers.TestStream, 0, 1, TextData.ValidName, TextData.ValidPdfFileName),
    ];

    [Test]
    public void MaxNumberOfFilesAttribute_ValidNumberOfFiles_ReturnsValid()
    {
        // Act
        var result = new MaxNumberOfFilesAttribute(FormFiles.Count).IsValid(FormFiles);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void MaxNumberOfFilesAttribute_TooManyFiles_ReturnsInvalid()
    {
        // Act
        var result = new MaxNumberOfFilesAttribute(FormFiles.Count - 1).IsValid(FormFiles);

        // Assert
        result.Should().BeFalse();
    }
}
