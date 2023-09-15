using Cts.AppServices.DtoBase;

namespace Cts.AppServices.ActionTypes;

public record ActionTypeViewDto(Guid Id, string Name, bool Active) : SimpleNamedEntityViewDto(Id, Name, Active);

public record ActionTypeCreateDto(string Name) : SimpleNamedEntityCreateDto(Name);

public record ActionTypeUpdateDto(Guid Id, string Name, bool Active) : SimpleNamedEntityUpdateDto(Id, Name, Active);
