using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Cts.AppServices.Attachments.ValidationAttributes;

/// <summary>
/// Validation attribute to limit the number of files that can be uploaded at a time.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
public class MinNumberOfFilesAttribute(int minNumberOfFiles = 0) : ValidationAttribute
{
    public override bool IsValid(object? value) =>
        value is not List<IFormFile> formFiles || formFiles.Count >= minNumberOfFiles;

    public override string FormatErrorMessage(string name) =>
        minNumberOfFiles switch
        {
            0 => "No files were selected",
            1 => "At least 1 file must be uploaded at a time.",
            _ => $"At least {minNumberOfFiles} files must be uploaded at a time.",
        };
}
