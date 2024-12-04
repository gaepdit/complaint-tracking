using Cts.AppServices.AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Cts.AppServices.RegisterServices;

public static class AutoMapperProfiles
{
    public static IServiceCollection AddAutoMapperProfiles(this IServiceCollection services)
    {
        // Add AutoMapper profiles
        services.AddAutoMapper(expression => expression.AddProfile<AutoMapperProfile>());
        return services;
    }
}
