using Cts.AppServices.Attachments;
using Cts.AppServices.ErrorLogging;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities.Attachments;
using Cts.Domain.Entities.Complaints;
using Cts.TestData.Constants;
using GaEpd.FileService;
using System.Text;

namespace AppServicesTests;

internal static class AppServiceHelpers
{
    public static byte[] TestData => Encoding.UTF8.GetBytes(TextData.ShortName);
    public static Stream TestStream => new MemoryStream(TestData);

    public static readonly IAttachmentService.AttachmentServiceConfig AttachmentServiceConfig =
        new(AttachmentsFolder: string.Empty, ThumbnailsFolder: string.Empty, ThumbnailSize: 99);

    public static AttachmentService BuildAttachmentService(
        IFileService? fileService = null,
        IAttachmentManager? attachmentManager = null,
        IAttachmentRepository? attachmentRepository = null,
        IComplaintRepository? complaintRepository = null,
        IUserService? userService = null) =>
        new(fileService ?? Substitute.For<IFileService>(),
            attachmentManager ?? Substitute.For<IAttachmentManager>(),
            attachmentRepository ?? Substitute.For<IAttachmentRepository>(),
            complaintRepository ?? Substitute.For<IComplaintRepository>(),
            userService ?? Substitute.For<IUserService>(),
            AppServicesTestsSetup.Mapper!,
            errorLogger: Substitute.For<IErrorLogger>());
}
