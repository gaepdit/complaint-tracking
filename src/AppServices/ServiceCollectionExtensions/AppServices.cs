using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MyAppRoot.AppServices.AutoMapper;
using MyAppRoot.AppServices.Offices;
using MyAppRoot.AppServices.Staff;
using MyAppRoot.Domain.Offices;

namespace MyAppRoot.AppServices.ServiceCollectionExtensions;

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
