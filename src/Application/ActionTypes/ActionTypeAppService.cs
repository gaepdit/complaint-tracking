using ComplaintTracking.Entities.ActionTypes;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace ComplaintTracking.ActionTypes;

public class ActionTypeAppService : CrudAppService<ActionType, ViewActionTypeDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateActionTypeDto>, IActionTypeAppService
{
    public ActionTypeAppService(IRepository<ActionType, Guid> repository) : base(repository) { }
}
