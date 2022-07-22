using ComplaintTracking.Entities.Concerns;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace ComplaintTracking.Concerns;

public class ConcernAppService : CrudAppService<Concern, ViewConcernDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateConcernDto>, IConcernAppService
{
    public ConcernAppService(IRepository<Concern, Guid> repository) : base(repository) { }
}
