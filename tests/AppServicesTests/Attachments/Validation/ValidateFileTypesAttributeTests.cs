using Cts.AppServices.Attachments.ValidationAttributes;
using Cts.TestData.Constants;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace AppServicesTests.Attachments.Validation;

[TestFixture]
[TestOf(typeof(ValidateFileTypesAttribute))]
public class ValidateFileTypesAttributeTests
{
    // Valid files

    private static byte[] TestTextData => Encoding.UTF8.GetBytes(TextData.ShortName);

    private static IFormFile TextFormFile => new FormFile(new MemoryStream(TestTextData), 0,
        TestTextData.Length, TextData.ValidName, TextData.ValidTextFileName);

    private static byte[] TestPdfData => [0x25, 0x50, 0x44, 0x46, 0x2D];

    private static IFormFile PdfFormFile => new FormFile(new MemoryStream(TestPdfData), 0,
        TestPdfData.Length, TextData.ValidName, TextData.ValidPdfFileName);

    // Invalid files

    private static IFormFile InvalidExtensionFormFile => new FormFile(new MemoryStream(TestTextData), 0,
        TestTextData.Length, TextData.ValidName, TextData.InValidFileName);

    private static byte[] TestZipData => [0x50, 0x4B, 0x03, 0x04];

    private static IFormFile ZipFormFile => new FormFile(new MemoryStream(TestZipData), 0, TestZipData.Length,
        TextData.ValidName, TextData.ValidPdfFileName);

    // Tests

    [Test]
    public void AllowedFileTypesAttribute_ValidTextFile_ReturnsValid()
    {
        // Act
        var result = new ValidateFileTypesAttribute().IsValid(TextFormFile);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void AllowedFileTypesAttribute_ValidTextFileCollection_ReturnsValid()
    {
        // Act
        var result = new ValidateFileTypesAttribute().IsValid(new List<IFormFile> { TextFormFile });

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void AllowedFileTypesAttribute_ValidPdfFile_ReturnsValid()
    {
        // Act
        var result = new ValidateFileTypesAttribute().IsValid(PdfFormFile);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void AllowedFileTypesAttribute_ValidPdfFileCollection_ReturnsValid()
    {
        // Act
        var result = new ValidateFileTypesAttribute().IsValid(new List<IFormFile> { PdfFormFile });

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void AllowedFileTypesAttribute_InvalidFileExtension_ReturnsInvalid()
    {
        // Act
        var result = new ValidateFileTypesAttribute().IsValid(InvalidExtensionFormFile);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public void AllowedFileTypesAttribute_InvalidFileExtensionCollection_ReturnsInvalid()
    {
        // Act
        var result = new ValidateFileTypesAttribute().IsValid(new List<IFormFile> { InvalidExtensionFormFile });

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public void AllowedFileTypesAttribute_InvalidFileSignature_ReturnsInvalid()
    {
        // Act
        var result = new ValidateFileTypesAttribute().IsValid(ZipFormFile);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public void AllowedFileTypesAttribute_InvalidFileSignatureCollection_ReturnsInvalid()
    {
        // Act
        var result = new ValidateFileTypesAttribute().IsValid(new List<IFormFile> { ZipFormFile });

        // Assert
        result.Should().BeFalse();
    }
}
