using AutoMapper;
using Cts.AppServices.Offices;
using Cts.AppServices.Staff;
using Cts.Domain.Identity;
using Cts.Domain.Offices;

namespace Cts.AppServices.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Office, OfficeViewDto>().ReverseMap();
        CreateMap<Office, OfficeUpdateDto>();

        CreateMap<ApplicationUser, StaffViewDto>().ReverseMap();
        CreateMap<ApplicationUser, StaffUpdateDto>();
    }
}
