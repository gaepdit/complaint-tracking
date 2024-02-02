using Cts.Domain.Entities.Attachments;
using Microsoft.AspNetCore.Http;

namespace Cts.AppServices.Attachments;

public interface IAttachmentFileService
{
    Task<byte[]> GetAttachmentFileAsync(string fileId, bool getThumbnail);
    Task DeleteAttachmentFileAsync(string fileId, bool isImage);
    Task<Attachment?> SaveAttachmentFileAsync(IFormFile formFile);
    FilesValidationResult ValidateUploadedFiles(List<IFormFile> formFiles);
}

public enum FilesValidationResult
{
    Valid,
    TooMany,
    WrongType,
}
