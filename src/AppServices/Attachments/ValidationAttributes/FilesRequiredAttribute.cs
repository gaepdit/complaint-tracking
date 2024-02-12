using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Cts.AppServices.Attachments.ValidationAttributes;

/// <summary>
/// Validation attribute to limit the number of files that can be uploaded at a time.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
public class FilesRequiredAttribute : ValidationAttribute
{
    public override bool IsValid(object? value) =>
        value is not List<IFormFile> formFiles || formFiles.Count > 0;

    public override string FormatErrorMessage(string name) => "No files were selected.";
}
