﻿using AutoMapper;
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
        CreateMap<ActionType, ActionTypeViewDto>();
        CreateMap<ActionType, ActionTypeUpdateDto>();

        CreateMap<ApplicationUser, StaffViewDto>();
        CreateMap<ApplicationUser, StaffSearchResultDto>();

        CreateMap<Attachment, AttachmentViewDto>();

        CreateMap<Complaint, ComplaintUpdateDto>()
            .ForMember(dto => dto.ReceivedDate, expression =>
                expression.MapFrom(complaint => DateOnly.FromDateTime(complaint.ReceivedDate.Date)))
            .ForMember(dto => dto.ReceivedTime, expression =>
                expression.MapFrom(complaint => TimeOnly.FromTimeSpan(complaint.ReceivedDate.TimeOfDay)))
            .ForMember(dto => dto.CurrentUserOfficeId, expression => expression.Ignore());
        CreateMap<Complaint, ComplaintPublicViewDto>();
        CreateMap<Complaint, ComplaintSearchResultDto>();
        CreateMap<Complaint, ComplaintViewDto>()
            .ForMember(dto => dto.CurrentUserOfficeId, expression => expression.Ignore());

        CreateMap<ComplaintAction, ComplaintActionPublicViewDto>();
        CreateMap<ComplaintAction, ComplaintActionViewDto>();
        CreateMap<ComplaintAction, ComplaintActionUpdateDto>();

        CreateMap<ComplaintTransition, ComplaintTransitionViewDto>();

        CreateMap<Concern, ConcernUpdateDto>();
        CreateMap<Concern, ConcernViewDto>();

        CreateMap<Office, OfficeUpdateDto>();
        CreateMap<Office, OfficeViewDto>();
        CreateMap<Office, OfficeWithAssignorDto>();
    }
}
