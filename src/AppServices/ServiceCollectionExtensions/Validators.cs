using Cts.AppServices.ActionTypes;
using Cts.AppServices.Offices;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Cts.AppServices.ServiceCollectionExtensions;

public static class Validators
{
    public static void AddValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<ActionTypeUpdateDto>, ActionTypeUpdateValidator>();
        services.AddScoped<IValidator<ActionTypeCreateDto>, ActionTypeCreateValidator>();

        services.AddScoped<IValidator<OfficeUpdateDto>, OfficeUpdateValidator>();
        services.AddScoped<IValidator<OfficeCreateDto>, OfficeCreateValidator>();
    }
}
