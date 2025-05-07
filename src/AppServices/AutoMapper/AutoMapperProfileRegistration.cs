using Microsoft.Extensions.DependencyInjection;

namespace Cts.AppServices.AutoMapper;

public static class AutoMapperProfileRegistration
{
    public static IServiceCollection AddAutoMapperProfiles(this IServiceCollection services) =>
        services.AddAutoMapper(expression => expression.AddProfile<AutoMapperProfile>());
}
