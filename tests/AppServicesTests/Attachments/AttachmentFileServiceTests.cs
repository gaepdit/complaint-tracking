using Cts.AppServices.Attachments;
using Cts.AppServices.ErrorLogging;
using Cts.Domain.Entities.Attachments;
using Cts.TestData.Constants;
using GaEpd.FileService;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace AppServicesTests.Attachments;

[TestFixture]
[TestOf(typeof(AttachmentFileService))]
public class AttachmentFileServiceTests
{
    private static readonly byte[] TestData = Encoding.UTF8.GetBytes(TextData.ShortName);
    private static readonly Stream TestStream = new MemoryStream(TestData);

    [Test]
    public async Task Get_FileExists_ReturnsByteArray()
    {
        // Arrange
        var response = new IFileService.TryGetResponse(TestStream);

        var fileService = Substitute.For<IFileService>();
        fileService.TryGetFileAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(response);

        var attachmentFileService = new AttachmentFileService(string.Empty, string.Empty, 99, fileService,
            Substitute.For<IAttachmentManager>(), Substitute.For<IErrorLogger>());

        // Act
        var result = await attachmentFileService.GetAttachmentFileAsync(TextData.ValidName, getThumbnail: false);

        // Assert
        result.Should().BeEquivalentTo(TestData);
    }

    [Test]
    public async Task Get_FileDoesNotExist_ReturnsEmptyByteArray()
    {
        // Arrange
        var fileService = Substitute.For<IFileService>();
        fileService.TryGetFileAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(IFileService.TryGetResponse.FailedTryGetResponse);

        var attachmentFileService = new AttachmentFileService(string.Empty, string.Empty, 99, fileService,
            Substitute.For<IAttachmentManager>(), Substitute.For<IErrorLogger>());

        // Act
        var result = await attachmentFileService.GetAttachmentFileAsync(TextData.ValidName, getThumbnail: false);

        // Assert
        result.Should().BeEquivalentTo(Array.Empty<byte>());
    }

    [Test]
    public async Task Save_EmptyFormFile_ReturnsNull()
    {
        // Arrange
        var formFile = Substitute.For<IFormFile>();
        formFile.Length.Returns(0);

        var attachmentFileService = new AttachmentFileService(string.Empty, string.Empty, 99,
            Substitute.For<IFileService>(), Substitute.For<IAttachmentManager>(), Substitute.For<IErrorLogger>());

        // Act
        var result = await attachmentFileService.SaveAttachmentFileAsync(formFile);

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public async Task Save_BlankFileName_ReturnsNull()
    {
        // Arrange
        var formFile = Substitute.For<IFormFile>();
        formFile.Length.Returns(1);
        formFile.FileName.Returns(string.Empty);

        var attachmentFileService = new AttachmentFileService(string.Empty, string.Empty, 99,
            Substitute.For<IFileService>(), Substitute.For<IAttachmentManager>(), Substitute.For<IErrorLogger>());

        // Act
        var result = await attachmentFileService.SaveAttachmentFileAsync(formFile);

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public async Task Save_NonImageFormFile_ReturnsAttachment()
    {
        // Arrange
        const int expectedSize = 1;
        const string expectedFileName = $"{TextData.ValidName}.pdf";
        const string expectedFileExtension = ".pdf";

        var formFile = Substitute.For<IFormFile>();
        formFile.Length.Returns(expectedSize);
        formFile.FileName.Returns(expectedFileName);
        formFile.OpenReadStream().Returns(Stream.Null);

        var attachmentFileService = new AttachmentFileService(string.Empty, string.Empty, 99,
            Substitute.For<IFileService>(), new AttachmentManager(), Substitute.For<IErrorLogger>());

        // Act
        var result = await attachmentFileService.SaveAttachmentFileAsync(formFile);

        // Assert
        using var _ = new AssertionScope();
        result!.Size.Should().Be(expectedSize);
        result.FileName.Should().Be(expectedFileName);
        result.FileExtension.Should().Be(expectedFileExtension);
        result.IsImage.Should().BeFalse();
        result.FileId.Should().Be($"{result.Id}{expectedFileExtension}");
    }

    [Test]
    public void ValidateUploadedFiles_Valid_ReturnsValidResult()
    {
        // Arrange
        List<IFormFile> formFiles =
        [
            new FormFile(TestStream, 0, 1, TextData.ValidName, ".pdf"),
        ];

        var attachmentFileService = new AttachmentFileService(string.Empty, string.Empty, 99,
            Substitute.For<IFileService>(), Substitute.For<IAttachmentManager>(), Substitute.For<IErrorLogger>());

        // Act
        var result = attachmentFileService.ValidateUploadedFiles(formFiles);

        // Assert
        result.Should().Be(FilesValidationResult.Valid);
    }

    [Test]
    public void ValidateUploadedFiles_ZeroLengthFile_ReturnsValidResult()
    {
        // Arrange
        List<IFormFile> formFiles =
        [
            new FormFile(Stream.Null, 0, 0, TextData.ValidName, "a.pdf"),
        ];

        var attachmentFileService = new AttachmentFileService(string.Empty, string.Empty, 99,
            Substitute.For<IFileService>(), Substitute.For<IAttachmentManager>(), Substitute.For<IErrorLogger>());

        // Act
        var result = attachmentFileService.ValidateUploadedFiles(formFiles);

        // Assert
        result.Should().Be(FilesValidationResult.Valid);
    }

    [Test]
    public void ValidateUploadedFiles_DisallowedType_ReturnsWrongTypeResult()
    {
        // Arrange
        List<IFormFile> formFiles =
        [
            new FormFile(TestStream, 0, 0, TextData.ValidName, ".zip"),
            new FormFile(TestStream, 0, 0, TextData.ValidName, ".pdf"),
        ];

        var attachmentFileService = new AttachmentFileService(string.Empty, string.Empty, 99,
            Substitute.For<IFileService>(), Substitute.For<IAttachmentManager>(), Substitute.For<IErrorLogger>());

        // Act
        var result = attachmentFileService.ValidateUploadedFiles(formFiles);

        // Assert
        result.Should().Be(FilesValidationResult.WrongType);
    }

    [Test]
    public void ValidateUploadedFiles_TooManyFiles_ReturnsTooManyResult()
    {
        // Arrange
        List<IFormFile> formFiles =
        [ // Eleven files is more than ten allowed.
            new FormFile(TestStream, 0, 1, TextData.ValidName, ".pdf"),
            new FormFile(TestStream, 0, 1, TextData.ValidName, ".pdf"),
            new FormFile(TestStream, 0, 1, TextData.ValidName, ".pdf"),
            new FormFile(TestStream, 0, 1, TextData.ValidName, ".pdf"),
            new FormFile(TestStream, 0, 1, TextData.ValidName, ".pdf"),
            new FormFile(TestStream, 0, 1, TextData.ValidName, ".pdf"),
            new FormFile(TestStream, 0, 1, TextData.ValidName, ".pdf"),
            new FormFile(TestStream, 0, 1, TextData.ValidName, ".pdf"),
            new FormFile(TestStream, 0, 1, TextData.ValidName, ".pdf"),
            new FormFile(TestStream, 0, 1, TextData.ValidName, ".pdf"),
            new FormFile(TestStream, 0, 1, TextData.ValidName, ".pdf"),
        ];

        var attachmentFileService = new AttachmentFileService(string.Empty, string.Empty, 99,
            Substitute.For<IFileService>(), Substitute.For<IAttachmentManager>(), Substitute.For<IErrorLogger>());

        // Act
        var result = attachmentFileService.ValidateUploadedFiles(formFiles);

        // Assert
        result.Should().Be(FilesValidationResult.TooMany);
    }
}
