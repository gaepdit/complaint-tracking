using Cts.AppServices.DtoBase;

namespace Cts.AppServices.Concerns;

public record ConcernViewDto(Guid Id, string Name, bool Active) : StandardNamedEntityViewDto(Id, Name, Active);

public record ConcernCreateDto(string Name) : StandardNamedEntityCreateDto(Name);

public record ConcernUpdateDto(Guid Id, string Name, bool Active) : StandardNamedEntityUpdateDto(Id, Name, Active);
