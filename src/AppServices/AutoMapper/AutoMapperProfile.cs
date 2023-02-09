using AutoMapper;
using Cts.AppServices.ActionTypes;
using Cts.AppServices.Attachments;
using Cts.AppServices.ComplaintActions;
using Cts.AppServices.Complaints.Dto;
using Cts.AppServices.Concerns;
using Cts.AppServices.Offices;
using Cts.AppServices.Staff;
using Cts.Domain.ActionTypes;
using Cts.Domain.Attachments;
using Cts.Domain.ComplaintActions;
using Cts.Domain.Complaints;
using Cts.Domain.Concerns;
using Cts.Domain.Identity;
using Cts.Domain.Offices;

namespace Cts.AppServices.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<ActionType, ActionTypeViewDto>();
        CreateMap<ActionType, ActionTypeUpdateDto>();

        CreateMap<ApplicationUser, StaffViewDto>().ReverseMap();
        CreateMap<ApplicationUser, StaffUpdateDto>();

        CreateMap<Attachment, AttachmentPublicViewDto>();

        CreateMap<Complaint, ComplaintPublicViewDto>();

        CreateMap<ComplaintAction, ComplaintActionPublicViewDto>();

        CreateMap<Concern, ConcernViewDto>();
        CreateMap<Concern, ConcernUpdateDto>();

        CreateMap<Office, OfficeViewDto>().ReverseMap();
        CreateMap<Office, OfficeUpdateDto>();
    }
}
