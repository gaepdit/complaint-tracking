﻿using Cts.AppServices.Attachments.ValidationAttributes;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Cts.AppServices.Attachments.Dto;

public record AttachmentsCreateDto(int ComplaintId)
{
    [Required(ErrorMessage = "No files were selected.")]
    [AllowedFileTypes]
    [NoEmptyFiles]
    [MaxNumberOfFiles(10)]
    [MinNumberOfFiles(1)]
    public List<IFormFile> FormFiles { get; init; } = [];
}
