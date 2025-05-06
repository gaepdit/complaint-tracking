using AutoMapper;
using Cts.AppServices.AuthenticationServices;
using Cts.AppServices.NamedEntities;
using Cts.Domain.Entities.Concerns;

namespace Cts.AppServices.Concerns;

public sealed class ConcernService(
    IConcernRepository repository,
    IConcernManager manager,
    IMapper mapper,
    IUserService userService)
    : NamedEntityService<Concern, ConcernViewDto, ConcernUpdateDto>
        (repository, manager, mapper, userService),
        IConcernService;
