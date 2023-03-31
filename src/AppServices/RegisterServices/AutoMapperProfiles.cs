using Cts.AppServices.AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Cts.AppServices.RegisterServices;

public static class AutoMapperProfiles
{
    public static void AddAutoMapperProfiles(this IServiceCollection services)
    {
        // Add AutoMapper profiles
        services.AddAutoMapper(cfg => cfg.AddProfile<AutoMapperProfile>());
    }
}
