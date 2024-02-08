using Cts.AppServices.Attachments;
using Cts.TestData.Constants;
using GaEpd.FileService;

namespace AppServicesTests.Attachments;

[TestFixture]
[TestOf(typeof(AttachmentService))]
public class GetAttachmentFileTests
{
    [Test]
    public async Task Get_FileExists_ReturnsByteArray()
    {
        // Arrange
        var response = new IFileService.TryGetResponse(AppServiceHelpers.TestStream);

        var fileService = Substitute.For<IFileService>();
        fileService.TryGetFileAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(response);

        var attachmentService = AppServiceHelpers.BuildAttachmentService(fileService: fileService);

        // Act
        var result = await attachmentService.GetAttachmentFileAsync(TextData.ValidName, getThumbnail: false,
            config: AppServiceHelpers.AttachmentServiceConfig);

        // Assert
        result.Should().BeEquivalentTo(AppServiceHelpers.TestData);
    }

    [Test]
    public async Task Get_FileDoesNotExist_ReturnsEmptyByteArray()
    {
        // Arrange
        var fileService = Substitute.For<IFileService>();
        fileService.TryGetFileAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(IFileService.TryGetResponse.FailedTryGetResponse);

        var attachmentService = AppServiceHelpers.BuildAttachmentService(fileService: fileService);

        // Act
        var result = await attachmentService.GetAttachmentFileAsync(TextData.ValidName, getThumbnail: false,
            config: AppServiceHelpers.AttachmentServiceConfig);

        // Assert
        result.Should().BeEquivalentTo(Array.Empty<byte>());
    }
}
