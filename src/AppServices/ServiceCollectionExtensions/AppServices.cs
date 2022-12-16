using Cts.AppServices.ActionTypes;
using Cts.AppServices.AutoMapper;
using Cts.AppServices.Complaints;
using Cts.AppServices.Concerns;
using Cts.AppServices.Offices;
using Cts.Domain.ActionTypes;
using Cts.Domain.Complaints;
using Cts.Domain.Concerns;
using Cts.Domain.Offices;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Cts.AppServices.ServiceCollectionExtensions;

public static class AppServices
{
    public static void AddAppServices(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => cfg.AddProfile<AutoMapperProfile>());

        // Action Types
        services.AddScoped<IActionTypeManager, ActionTypeManager>();
        services.AddScoped<IActionTypeAppService, ActionTypeAppService>();

        // Complaints
        services.AddScoped<IComplaintAppService, ComplaintAppService>();
        services.AddScoped<IComplaintManager, ComplaintManager>();

        // Concerns
        services.AddScoped<IConcernManager, ConcernManager>();
        services.AddScoped<IConcernAppService, ConcernAppService>();

        // Offices
        services.AddScoped<IOfficeManager, OfficeManager>();
        services.AddScoped<IOfficeAppService, OfficeAppService>();

        // Add all validators
        services.AddValidatorsFromAssemblyContaining(typeof(AppServices));
    }
}
