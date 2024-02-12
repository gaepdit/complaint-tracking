using Cts.AppServices.Attachments.ValidationAttributes;
using Cts.TestData.Constants;
using Microsoft.AspNetCore.Http;

namespace AppServicesTests.Attachments.Validation;

[TestFixture]
[TestOf(typeof(FilesRequiredAttribute))]
public class FilesRequiredAttributeTests
{
    private static List<IFormFile> FormFiles =>
    [
        new FormFile(AppServiceHelpers.TestStream, 0, 1, TextData.ValidName, TextData.ValidPdfFileName),
    ];

    [Test]
    public void MinNumberOfFilesAttribute_ValidNumberOfFiles_ReturnsValid()
    {
        // Act
        var result = new FilesRequiredAttribute().IsValid(FormFiles);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void MinNumberOfFilesAttribute_NotEnoughFiles_ReturnsInvalid()
    {
        // Act
        var result = new FilesRequiredAttribute().IsValid(new List<IFormFile>());

        // Assert
        result.Should().BeFalse();
    }
}
