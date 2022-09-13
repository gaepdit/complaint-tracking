using AutoMapper;
using Cts.AppServices.ActionTypes;
using Cts.AppServices.Offices;
using Cts.AppServices.Staff;
using Cts.Domain.Entities;

namespace Cts.AppServices.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<ActionType, ActionTypeViewDto>();
        CreateMap<ActionType, ActionTypeUpdateDto>();

        CreateMap<Office, OfficeViewDto>().ReverseMap();
        CreateMap<Office, OfficeUpdateDto>();

        CreateMap<ApplicationUser, StaffViewDto>().ReverseMap();
        CreateMap<ApplicationUser, StaffUpdateDto>();
    }
}
