using FluentValidation;
using FluentValidation.Results;
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
    /// <typeparam name="T">The type of the item to be validated.</typeparam>
    public static async Task ApplyValidationAsync<T>(this IValidator<T> validator, T item,
        ModelStateDictionary modelState, object? dataValue = null,
        [CallerArgumentExpression(nameof(dataValue))] string? dataKey = null)
    {
        var validationResult = dataValue == null || dataKey == null
            ? await validator.ValidateAsync(item)
            : await validator.ValidateAsync(new ValidationContext<T>(item)
                { RootContextData = { [dataKey] = dataValue } });

        if (!validationResult.IsValid) validationResult.AddToModelState(modelState);
    }

    /// <summary>
    /// Stores the errors in a <see cref="ValidationResult"/> object to the specified <see cref="ModelStateDictionary"/>.
    /// </summary>
    /// <param name="result">The ValidationResult to store.</param>
    /// <param name="modelState">The ModelStateDictionary to store the errors in.</param>
    private static void AddToModelState(this ValidationResult result, ModelStateDictionary modelState)
    {
        foreach (var error in result.Errors)
            modelState.AddModelError(error.PropertyName, error.ErrorMessage);
    }
}
