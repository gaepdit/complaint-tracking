﻿using AutoMapper;
using Cts.AppServices.ActionTypes;
using Cts.AppServices.Attachments;
using Cts.AppServices.ComplaintActions;
using Cts.AppServices.Complaints.Dto;
using Cts.AppServices.Concerns;
using Cts.AppServices.Offices;
using Cts.AppServices.Staff;
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
        CreateMap<ApplicationUser, StaffUpdateDto>();

        CreateMap<Attachment, AttachmentPublicViewDto>();
        CreateMap<Attachment, AttachmentViewDto>();

        CreateMap<Complaint, ComplaintPublicViewDto>();
        CreateMap<Complaint, ComplaintViewDto>()
            .ForMember(d => d.CurrentUserOfficeId, o => o.Ignore());
        CreateMap<Complaint, ComplaintSearchResultDto>();

        CreateMap<ComplaintAction, ComplaintActionPublicViewDto>();
        CreateMap<ComplaintAction, ComplaintActionViewDto>();

        CreateMap<ComplaintTransition, ComplaintTransitionViewDto>();

        CreateMap<Concern, ConcernViewDto>();
        CreateMap<Concern, ConcernUpdateDto>();

        CreateMap<Office, OfficeDisplayViewDto>();
        CreateMap<Office, OfficeAdminViewDto>();
        CreateMap<Office, OfficeUpdateDto>();
    }
}
