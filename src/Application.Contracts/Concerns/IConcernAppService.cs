using Volo.Abp.Application.Services;

namespace ComplaintTracking.Concerns;

public interface IConcernAppService :
    ICrudAppService<ViewConcernDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateConcernDto>
{ }
