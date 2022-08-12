using AutoMapper;
using Cts.AppServices.ActionTypes;
using Cts.AppServices.Offices;
using Cts.AppServices.Users;
using Cts.Domain.Entities;
using Cts.Domain.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Cts.AppServices.ServiceCollectionExtensions;

public static class AutoMapper
{
    public static void AddAppAutoMapperProfiles(this IServiceCollection services) =>
        services.AddAutoMapper(cfg => cfg.AddProfile<AutoMapperProfiles>());
}

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<ActionType, ActionTypeViewDto>();
        CreateMap<ActionType, ActionTypeUpdateDto>();

        CreateMap<Office, OfficeViewDto>();
        CreateMap<Office, OfficeUpdateDto>();

        CreateMap<ApplicationUser, UserViewDto>();
    }
}
