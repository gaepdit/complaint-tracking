using Cts.AppServices.Attachments;
using Cts.AppServices.Attachments.Dto;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities.Attachments;
using Cts.Domain.Entities.Complaints;
using Cts.Domain.Identity;
using Cts.TestData.Constants;
using GaEpd.FileService;
using Microsoft.AspNetCore.Http;

namespace AppServicesTests.Attachments;

[TestFixture]
[TestOf(typeof(AttachmentService))]
public class SaveAttachmentTests
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

    [Test]
    public async Task Save_ValidFormFile_TakesCorrectAction()
    {
        // Arrange
        var formFile = Substitute.For<IFormFile>();
        formFile.Length.Returns(1);
        formFile.FileName.Returns(TextData.ValidPdfFileName);
        var dto = new AttachmentsCreateDto(1) { FormFiles = [formFile] };

        var complaintRepository = Substitute.For<IComplaintRepository>();
        complaintRepository.GetAsync(1, Arg.Any<CancellationToken>())
            .Returns(new Complaint(1));

        var userService = Substitute.For<IUserService>();
        userService.GetCurrentUserAsync().Returns((ApplicationUser?)null);

        var attachment = new Attachment(Guid.NewGuid())
        {
            Complaint = new Complaint(1),
            FileName = TextData.ValidPdfFileName,
            FileExtension = TextData.ValidPdfFileExtension,
            Size = 1,
        };

        var attachmentManager = Substitute.For<IAttachmentManager>();
        attachmentManager.Create(Arg.Any<IFormFile>(), Arg.Any<Complaint>(), Arg.Any<ApplicationUser?>())
            .Returns(attachment);

        var attachmentRepository = Substitute.For<IAttachmentRepository>();

        var attachmentService = AppServiceHelpers.BuildAttachmentService(attachmentManager: attachmentManager,
            attachmentRepository: attachmentRepository, complaintRepository: complaintRepository,
            userService: userService);

        // Act
        await attachmentService.SaveAttachmentsAsync(dto, AppServiceHelpers.AttachmentServiceConfig);

        // Assert
        attachmentManager.Received().Create(Arg.Any<IFormFile>(), Arg.Any<Complaint>(), Arg.Any<ApplicationUser?>());
        await attachmentRepository.Received()
            .InsertAsync(Arg.Any<Attachment>(), Arg.Any<bool>(), Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Save_EmptyFormFile_TakesNoAction()
    {
        // Arrange
        var formFile = Substitute.For<IFormFile>();
        formFile.Length.Returns(0);
        var dto = new AttachmentsCreateDto(1) { FormFiles = [formFile] };

        var complaintRepository = Substitute.For<IComplaintRepository>();
        complaintRepository.GetAsync(1, Arg.Any<CancellationToken>())
            .Returns(new Complaint(1));

        var userService = Substitute.For<IUserService>();
        userService.GetCurrentUserAsync().Returns((ApplicationUser?)null);

        var attachmentManager = Substitute.For<IAttachmentManager>();

        var attachmentRepository = Substitute.For<IAttachmentRepository>();

        var attachmentService = AppServiceHelpers.BuildAttachmentService(attachmentManager: attachmentManager,
            attachmentRepository: attachmentRepository, complaintRepository: complaintRepository,
            userService: userService);

        // Act
        await attachmentService.SaveAttachmentsAsync(dto, AppServiceHelpers.AttachmentServiceConfig);

        // Assert
        attachmentManager.ReceivedCalls().Should().BeEmpty();
        attachmentRepository.ReceivedCalls().Should().BeEmpty();
    }

    [Test]
    public async Task Save_BlankFileName_TakesNoAction()
    {
        // Arrange
        var formFile = Substitute.For<IFormFile>();
        formFile.Length.Returns(1);
        formFile.FileName.Returns(string.Empty);
        var dto = new AttachmentsCreateDto(1) { FormFiles = [formFile] };

        var complaintRepository = Substitute.For<IComplaintRepository>();
        complaintRepository.GetAsync(1, Arg.Any<CancellationToken>())
            .Returns(new Complaint(1));

        var userService = Substitute.For<IUserService>();
        userService.GetCurrentUserAsync().Returns((ApplicationUser?)null);

        var attachmentManager = Substitute.For<IAttachmentManager>();

        var attachmentRepository = Substitute.For<IAttachmentRepository>();

        var attachmentService = AppServiceHelpers.BuildAttachmentService(attachmentManager: attachmentManager,
            attachmentRepository: attachmentRepository, complaintRepository: complaintRepository,
            userService: userService);

        // Act
        await attachmentService.SaveAttachmentsAsync(dto, AppServiceHelpers.AttachmentServiceConfig);

        // Assert
        attachmentManager.ReceivedCalls().Should().BeEmpty();
        attachmentRepository.ReceivedCalls().Should().BeEmpty();
    }
}
