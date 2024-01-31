using AutoMapper;
using Cts.AppServices.ServiceBase;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities.ActionTypes;

namespace Cts.AppServices.ActionTypes;

public sealed class ActionTypeService(
    IActionTypeRepository repository,
    IActionTypeManager manager,
    IMapper mapper,
    IUserService userService)
    : MaintenanceItemService<ActionType, ActionTypeViewDto, ActionTypeUpdateDto>
        (repository, manager, mapper, userService),
        IActionTypeService;
