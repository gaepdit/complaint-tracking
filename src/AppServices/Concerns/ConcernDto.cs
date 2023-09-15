using Cts.AppServices.DtoBase;

namespace Cts.AppServices.Concerns;

public record ConcernViewDto(Guid Id, string Name, bool Active) : SimpleNamedEntityViewDto(Id, Name, Active);

public record ConcernCreateDto(string Name) : SimpleNamedEntityCreateDto(Name);

public record ConcernUpdateDto(Guid Id, string Name, bool Active) : SimpleNamedEntityUpdateDto(Id, Name, Active);
