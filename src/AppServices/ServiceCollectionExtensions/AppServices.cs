using Cts.AppServices.AutoMapper;
using Cts.AppServices.Offices;
using Cts.AppServices.Staff;
using Cts.Domain.Offices;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Cts.AppServices.ServiceCollectionExtensions;

public static class AppServices
{
    public static void AddAppServices(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => cfg.AddProfile<AutoMapperProfile>());

        // Offices
        services.AddScoped<IOfficeManager, OfficeManager>();
        services.AddScoped<IOfficeAppService, OfficeAppService>();
        services.AddScoped<IValidator<OfficeUpdateDto>, OfficeUpdateValidator>();
        services.AddScoped<IValidator<OfficeCreateDto>, OfficeCreateValidator>();

        // Staff
        services.AddScoped<IValidator<StaffUpdateDto>, StaffUpdateValidator>();
    }
}
