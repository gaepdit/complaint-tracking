using Cts.AppServices.Attachments;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Runtime.CompilerServices;

namespace Cts.WebApp.Platform.PageModelHelpers;

public static class ValidationHelper
{
    /// <summary>
    /// Applies the registered validator to the item. If any validation errors result, they are added to the model state.
    /// </summary>
    /// <param name="validator">The <see cref="IValidator{T}"/> for type <see cref="T"/>.</param>
    /// <param name="item">The object to be validated.</param>
    /// <param name="modelState">The <see cref="ModelStateDictionary"/> to add any validation errors to.</param>
    /// <param name="dataValue">An optional data value that will be made available to the validator as part of the <see cref="ValidationContext{T}"/>.</param>
    /// <param name="dataKey">The key for the data value.</param>
    /// <param name="parameterName">The name of the item parameter.</param>
    /// <typeparam name="T">The type of the item to be validated.</typeparam>
    public static async Task ApplyValidationAsync<T>(this IValidator<T> validator, T item,
        ModelStateDictionary modelState, object? dataValue = null,
        [CallerArgumentExpression(nameof(dataValue))]
        string? dataKey = null,
        [CallerArgumentExpression(nameof(item))]
        string? parameterName = null)
    {
        var validationResult = dataValue == null || dataKey == null
            ? await validator.ValidateAsync(item)
            : await validator.ValidateAsync(new ValidationContext<T>(item)
                { RootContextData = { [dataKey] = dataValue } });

        if (!validationResult.IsValid) validationResult.AddToModelState(modelState, parameterName);
    }

    // ReSharper disable once ConvertIfStatementToSwitchStatement
    public static void ValidateUploadedFiles(this List<IFormFile> formFiles, ModelStateDictionary modelState)
    {
        if (formFiles.Count == 0)
            modelState.AddModelError(string.Empty, "No files were selected.");

        else if (formFiles.Count > 10)
            modelState.AddModelError(string.Empty,
                "No more than ten files may be uploaded at a time.");

        if (formFiles.Exists(file => !FileTypes.FileUploadAllowed(file.FileName)))
            modelState.AddModelError(string.Empty, "Invalid file type selected.");
    }
}
