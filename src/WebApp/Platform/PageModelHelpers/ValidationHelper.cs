using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Runtime.CompilerServices;

namespace Cts.WebApp.Platform.PageModelHelpers;

public static class ValidationHelper
{
    public static async Task ApplyValidationAsync<T>(
        this IValidator<T> validator,
        T item,
        ModelStateDictionary modelState,
        [CallerArgumentExpression("item")] string? parameterName = null)
    {
        var validationResult = await validator.ValidateAsync(item);
        if (!validationResult.IsValid) validationResult.AddToModelState(modelState, parameterName);
    }
}
