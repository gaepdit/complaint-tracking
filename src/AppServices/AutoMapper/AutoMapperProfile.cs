using AutoMapper;
using Cts.AppServices.ActionTypes;
using Cts.AppServices.Attachments;
using Cts.AppServices.ComplaintActions;
using Cts.AppServices.Complaints.Dto;
using Cts.AppServices.ComplaintTransitions;
using Cts.AppServices.Concerns;
using Cts.AppServices.Offices;
using Cts.AppServices.Staff;
using Cts.Domain.ActionTypes;
using Cts.Domain.Attachments;
using Cts.Domain.ComplaintActions;
using Cts.Domain.Complaints;
using Cts.Domain.ComplaintTransitions;
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
        CreateMap<Attachment, AttachmentViewDto>();

        CreateMap<Complaint, ComplaintPublicViewDto>();
        CreateMap<Complaint, ComplaintViewDto>();
        CreateMap<Complaint, ComplaintSearchResultDto>();

        CreateMap<ComplaintCreateDto, Complaint>(MemberList.Source)
            .ForMember(d => d.DateReceived,
                o => o.MapFrom(s => s.DateReceived.Add(s.TimeReceived.TimeOfDay)))
            .ForSourceMember(s => s.DateReceived, o => o.DoNotValidate())
            .ForSourceMember(s => s.TimeReceived, o => o.DoNotValidate());

        CreateMap<ComplaintAction, ComplaintActionPublicViewDto>();
        CreateMap<ComplaintAction, ComplaintActionViewDto>();

        CreateMap<ComplaintTransition, ComplaintTransitionViewDto>();

        CreateMap<Concern, ConcernViewDto>();
        CreateMap<Concern, ConcernUpdateDto>();

        CreateMap<Office, OfficeDisplayViewDto>().ReverseMap();
        CreateMap<Office, OfficeAdminViewDto>();
        CreateMap<Office, OfficeUpdateDto>();
    }
}
