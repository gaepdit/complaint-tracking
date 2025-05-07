using AutoMapper;
using Cts.AppServices.IdentityServices;
using Cts.AppServices.NamedEntities;
using Cts.Domain.Entities.ActionTypes;

namespace Cts.AppServices.ActionTypes;

public sealed class ActionTypeService(
    IActionTypeRepository repository,
    IActionTypeManager manager,
    IMapper mapper,
    IUserService userService)
    : NamedEntityService<ActionType, ActionTypeViewDto, ActionTypeUpdateDto>
        (repository, manager, mapper, userService),
        IActionTypeService;
