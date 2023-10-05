using Cts.AppServices.DtoBase;

namespace Cts.AppServices.ActionTypes;

public record ActionTypeViewDto(Guid Id, string Name, bool Active) : StandardNamedEntityViewDto(Id, Name, Active);

public record ActionTypeCreateDto(string Name) : StandardNamedEntityCreateDto(Name);

public record ActionTypeUpdateDto(string Name, bool Active) : StandardNamedEntityUpdateDto(Name, Active);
