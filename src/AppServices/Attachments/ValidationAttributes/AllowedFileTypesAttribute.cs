using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Cts.AppServices.Attachments.ValidationAttributes;

/// <summary>
/// Validation attribute to limit the file types that can be selected for file uploads.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
public class AllowedFileTypesAttribute : ValidationAttribute
{
    public override bool IsValid(object? value) =>
        value switch
        {
            IFormFile file => FileTypeValidation.IsFileSignatureValid(file),
            List<IFormFile> formFiles => formFiles.TrueForAll(FileTypeValidation.IsFileSignatureValid),
            _ => true,
        };

    public override string FormatErrorMessage(string name) =>
        "Invalid file type selected.";
}
