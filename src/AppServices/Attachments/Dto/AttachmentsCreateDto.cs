using Cts.AppServices.Attachments.ValidationAttributes;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Cts.AppServices.Attachments.Dto;

public record AttachmentsCreateDto(int ComplaintId)
{
    [Required(ErrorMessage = "No files were selected.")]
    [ValidateFileTypes]
    [FilesNotEmpty]
    [FilesRequired]
    [MaxNumberOfFiles(IAttachmentService.MaxSimultaneousUploads)]
    public List<IFormFile> Files { get; } = [];
}
