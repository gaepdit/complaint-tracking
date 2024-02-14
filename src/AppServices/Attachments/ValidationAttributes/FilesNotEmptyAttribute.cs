using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Cts.AppServices.Attachments.ValidationAttributes;

/// <summary>
/// Validation attribute to require that file uploads are not empty.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
public class FilesNotEmptyAttribute : ValidationAttribute
{
    public override bool IsValid(object? value) =>
        value switch
        {
            IFormFile file => file.FileIsNotEmpty(),
            List<IFormFile> formFiles => formFiles.TrueForAll(ValidateFiles.FileIsNotEmpty),
            _ => true,
        };

    public override string FormatErrorMessage(string name) => ValidateFiles.EmptyFileErrorMessage;
}
