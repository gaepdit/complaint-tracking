using Volo.Abp.Application.Services;

namespace ComplaintTracking.ActionTypes;

public interface IActionTypeAppService :
    ICrudAppService<ViewActionTypeDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateActionTypeDto>
{ }
