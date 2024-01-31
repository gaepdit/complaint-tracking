using AutoMapper;
using Cts.AppServices.ServiceBase;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities.Concerns;

namespace Cts.AppServices.Concerns;

public sealed class ConcernService(
    IConcernRepository repository,
    IConcernManager manager,
    IMapper mapper,
    IUserService userService)
    : MaintenanceItemService<Concern, ConcernViewDto, ConcernUpdateDto>
        (repository, manager, mapper, userService),
        IConcernService;
