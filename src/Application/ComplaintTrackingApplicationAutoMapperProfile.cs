using AutoMapper;
using ComplaintTracking.ActionTypes;
using ComplaintTracking.Concerns;
using ComplaintTracking.Entities.ActionTypes;
using ComplaintTracking.Entities.Concerns;

namespace ComplaintTracking;

public class ComplaintTrackingApplicationAutoMapperProfile : Profile
{
    public ComplaintTrackingApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<Concern, ViewConcernDto>();
        CreateMap<CreateUpdateConcernDto, Concern>();

        CreateMap<ActionType, ViewActionTypeDto>();
        CreateMap<CreateUpdateActionTypeDto, ActionType>();
    }
}
