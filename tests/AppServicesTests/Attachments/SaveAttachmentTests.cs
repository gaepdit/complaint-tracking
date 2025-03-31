using Cts.AppServices.Attachments;
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
        fileService.TryGetFileAsync(Arg.Any<string>(), Arg.Any<string>())
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
        fileService.TryGetFileAsync(Arg.Any<string>(), Arg.Any<string>())
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
        const int complaintId = 1;

        var formFile = Substitute.For<IFormFile>();
        formFile.Length.Returns(1);
        formFile.FileName.Returns(TextData.ValidPdfFileName);

        var complaintRepository = Substitute.For<IComplaintRepository>();
        complaintRepository.GetAsync(complaintId)
            .Returns(new Complaint(complaintId));

        var userService = Substitute.For<IUserService>();
        userService.GetCurrentUserAsync().Returns((ApplicationUser?)null);

        var attachment = new Attachment(Guid.NewGuid())
        {
            Complaint = new Complaint(complaintId),
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
        await attachmentService.SaveAttachmentsAsync(complaintId, [formFile], AppServiceHelpers.AttachmentServiceConfig);

        // Assert
        attachmentManager.Received().Create(Arg.Any<IFormFile>(), Arg.Any<Complaint>(), Arg.Any<ApplicationUser?>());
        await attachmentRepository.Received()
            .InsertAsync(Arg.Any<Attachment>(), Arg.Any<bool>());
    }

    [Test]
    public async Task Save_EmptyFormFile_TakesNoAction()
    {
        // Arrange
        const int complaintId = 1;

        var formFile = Substitute.For<IFormFile>();
        formFile.Length.Returns(0);

        var complaintRepository = Substitute.For<IComplaintRepository>();
        complaintRepository.GetAsync(complaintId)
            .Returns(new Complaint(complaintId));

        var userService = Substitute.For<IUserService>();
        userService.GetCurrentUserAsync().Returns((ApplicationUser?)null);

        var attachmentManager = Substitute.For<IAttachmentManager>();

        var attachmentRepository = Substitute.For<IAttachmentRepository>();

        var attachmentService = AppServiceHelpers.BuildAttachmentService(attachmentManager: attachmentManager,
            attachmentRepository: attachmentRepository, complaintRepository: complaintRepository,
            userService: userService);

        // Act
        await attachmentService.SaveAttachmentsAsync(complaintId, [formFile], AppServiceHelpers.AttachmentServiceConfig);

        // Assert
        attachmentManager.ReceivedCalls().Should().BeEmpty();
        await attachmentRepository.DidNotReceiveWithAnyArgs()
            .InsertAsync(Arg.Any<Attachment>(), Arg.Any<bool>());
    }

    [Test]
    public async Task Save_BlankFileName_TakesNoAction()
    {
        // Arrange
        const int complaintId = 1;

        var formFile = Substitute.For<IFormFile>();
        formFile.Length.Returns(1);
        formFile.FileName.Returns(string.Empty);

        var complaintRepository = Substitute.For<IComplaintRepository>();
        complaintRepository.GetAsync(complaintId)
            .Returns(new Complaint(complaintId));

        var userService = Substitute.For<IUserService>();
        userService.GetCurrentUserAsync().Returns((ApplicationUser?)null);

        var attachmentManager = Substitute.For<IAttachmentManager>();

        var attachmentRepository = Substitute.For<IAttachmentRepository>();

        var attachmentService = AppServiceHelpers.BuildAttachmentService(attachmentManager: attachmentManager,
            attachmentRepository: attachmentRepository, complaintRepository: complaintRepository,
            userService: userService);

        // Act
        await attachmentService.SaveAttachmentsAsync(complaintId,[formFile], AppServiceHelpers.AttachmentServiceConfig);

        // Assert
        attachmentManager.ReceivedCalls().Should().BeEmpty();
        await attachmentRepository.DidNotReceiveWithAnyArgs()
            .InsertAsync(Arg.Any<Attachment>(), Arg.Any<bool>());
    }
}
