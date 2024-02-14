using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Cts.AppServices.Attachments.ValidationAttributes;

/// <summary>
/// Validation attribute to limit the file types that can be selected for file uploads.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
public class ValidateFileTypesAttribute : ValidationAttribute
{
    public override bool IsValid(object? value) =>
        value switch
        {
            IFormFile file => file.IsFileSignatureValid(),
            List<IFormFile> formFiles => formFiles.TrueForAll(ValidateFiles.IsFileSignatureValid),
            _ => true,
        };

    public override string FormatErrorMessage(string name) => ValidateFiles.InvalidFileTypeErrorMessage;
}
