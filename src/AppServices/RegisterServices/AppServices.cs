using Cts.AppServices.ActionTypes;
using Cts.AppServices.Complaints;
using Cts.AppServices.Concerns;
using Cts.AppServices.Offices;
using Cts.Domain.ActionTypes;
using Cts.Domain.Complaints;
using Cts.Domain.Concerns;
using Cts.Domain.Offices;
using Microsoft.Extensions.DependencyInjection;

namespace Cts.AppServices.RegisterServices;

public static class AppServices
{
    public static void AddAppServices(this IServiceCollection services)
    {
        // Action Types
        services.AddScoped<IActionTypeManager, ActionTypeManager>();
        services.AddScoped<IActionTypeAppService, ActionTypeAppService>();

        // Complaints
        services.AddScoped<IComplaintManager, ComplaintManager>();
        services.AddScoped<IComplaintAppService, ComplaintAppService>();

        // Concerns
        services.AddScoped<IConcernManager, ConcernManager>();
        services.AddScoped<IConcernAppService, ConcernAppService>();

        // Offices
        services.AddScoped<IOfficeManager, OfficeManager>();
        services.AddScoped<IOfficeAppService, OfficeAppService>();
    }
}
