using AutoMapper;
using Cts.AppServices.ActionTypes;
using Cts.AppServices.Attachments.Dto;
using Cts.AppServices.ComplaintActions.Dto;
using Cts.AppServices.Complaints.CommandDto;
using Cts.AppServices.Complaints.QueryDto;
using Cts.AppServices.Concerns;
using Cts.AppServices.Offices;
using Cts.AppServices.Staff.Dto;
using Cts.Domain.Entities.ActionTypes;
using Cts.Domain.Entities.Attachments;
using Cts.Domain.Entities.ComplaintActions;
using Cts.Domain.Entities.Complaints;
using Cts.Domain.Entities.ComplaintTransitions;
using Cts.Domain.Entities.Concerns;
using Cts.Domain.Entities.Offices;
using Cts.Domain.Identity;

namespace Cts.AppServices.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<ActionType, ActionTypeUpdateDto>();
        CreateMap<ActionType, ActionTypeViewDto>();

        CreateMap<ApplicationUser, StaffSearchResultDto>();
        CreateMap<ApplicationUser, StaffViewDto>();

        CreateMap<Attachment, AttachmentViewDto>()
            .ForMember(dto => dto.IsForPublic, expression => expression.Ignore());

        CreateMap<Complaint, ComplaintPublicViewDto>();
        CreateMap<Complaint, ComplaintSearchResultDto>();
        CreateMap<Complaint, ComplaintUpdateDto>()
            .ForMember(dto => dto.ReceivedDate, expression =>
                expression.MapFrom(complaint => DateOnly.FromDateTime(complaint.ReceivedDate.Date)))
            .ForMember(dto => dto.ReceivedTime, expression =>
                expression.MapFrom(complaint => TimeOnly.FromTimeSpan(complaint.ReceivedDate.TimeOfDay)));
        CreateMap<Complaint, ComplaintViewDto>();

        CreateMap<ComplaintAction, ActionPublicViewDto>();
        CreateMap<ComplaintAction, ActionSearchResultDto>();
        CreateMap<ComplaintAction, ActionUpdateDto>();
        CreateMap<ComplaintAction, ActionViewDto>();

        CreateMap<ComplaintTransition, ComplaintTransitionViewDto>();

        CreateMap<Concern, ConcernUpdateDto>();
        CreateMap<Concern, ConcernViewDto>();

        CreateMap<Office, OfficeUpdateDto>();
        CreateMap<Office, OfficeViewDto>();
        CreateMap<Office, OfficeWithAssignorDto>();
    }
}
