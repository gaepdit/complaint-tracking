using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Cts.AppServices.Attachments.ValidationAttributes;

/// <summary>
/// Validation attribute to require that file uploads are not empty.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
public class NoEmptyFilesAttribute : ValidationAttribute
{
    public override bool IsValid(object? value) =>
        value switch
        {
            IFormFile file => file.Length > 0,
            List<IFormFile> formFiles => formFiles.TrueForAll(file => file.Length > 0),
            _ => true,
        };

    public override string FormatErrorMessage(string name) =>
        "Empty file selected.";
}
