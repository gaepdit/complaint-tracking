using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Cts.AppServices.Attachments.Dto;

public record AttachmentsCreateDto(int ComplaintId)
{
    [Required(ErrorMessage = "No files selected.")]
    public List<IFormFile> FormFiles { get; init; } = default!;
}
