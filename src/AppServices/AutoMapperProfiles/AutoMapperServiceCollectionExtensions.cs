using Microsoft.Extensions.DependencyInjection;

namespace Cts.AppServices.AutoMapperProfiles;

public static class AutoMapperServiceCollectionExtensions
{
    public static void AddAppAutoMapperProfiles(this IServiceCollection services) => 
        services.AddAutoMapper(cfg => cfg.AddProfile<AutoMapperProfiles>());
}
