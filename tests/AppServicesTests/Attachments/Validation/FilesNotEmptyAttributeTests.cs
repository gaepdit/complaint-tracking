using Cts.AppServices.Attachments.ValidationAttributes;
using Cts.TestData.Constants;
using Microsoft.AspNetCore.Http;

namespace AppServicesTests.Attachments.Validation;

[TestFixture]
[TestOf(typeof(FilesNotEmptyAttribute))]
public class FilesNotEmptyAttributeTests
{
    private static readonly IFormFile EmptyFormFile =
        new FormFile(Stream.Null, 0, 0, TextData.ValidName, TextData.ValidPdfFileName);

    private static readonly IFormFile NonEmptyFormFile =
        new FormFile(AppServiceHelpers.TestStream, 0, 1, TextData.ValidName, TextData.ValidPdfFileName);

    private static List<IFormFile> EmptyFormFiles =>
    [
        NonEmptyFormFile,
        EmptyFormFile,
    ];

    private static List<IFormFile> NonEmptyFormFiles =>
    [
        NonEmptyFormFile,
        NonEmptyFormFile,
    ];

    [Test]
    public void NoEmptyFilesAttribute_NonEmptyFile_ReturnsValid()
    {
        // Act
        var result = new FilesNotEmptyAttribute().IsValid(NonEmptyFormFile);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void NoEmptyFilesAttribute_NonEmptyFileCollection_ReturnsValid()
    {
        // Act
        var result = new FilesNotEmptyAttribute().IsValid(NonEmptyFormFiles);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void NoEmptyFilesAttribute_EmptyFile_ReturnsInvalid()
    {
        // Act
        var result = new FilesNotEmptyAttribute().IsValid(EmptyFormFile);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public void NoEmptyFilesAttribute_EmptyFileCollection_ReturnsInvalid()
    {
        // Act
        var result = new FilesNotEmptyAttribute().IsValid(EmptyFormFiles);

        // Assert
        result.Should().BeFalse();
    }
}
