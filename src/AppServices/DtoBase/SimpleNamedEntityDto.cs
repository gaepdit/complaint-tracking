using Cts.Domain.Entities.EntityBase;
using System.ComponentModel.DataAnnotations;

namespace Cts.AppServices.DtoBase;

public interface IDtoHasNameProperty
{
    string Name { get; }
}

public abstract record SimpleNamedEntityViewDto
(
    Guid Id,
    string Name,
    bool Active
) : IDtoHasNameProperty;

public abstract record SimpleNamedEntityCreateDto
(
    [Required(AllowEmptyStrings = false)]
    [StringLength(SimpleNamedEntity.MaxNameLength, MinimumLength = SimpleNamedEntity.MinNameLength)]
    string Name
);

public abstract record SimpleNamedEntityUpdateDto
(
    Guid Id,
    [Required(AllowEmptyStrings = false)]
    [StringLength(SimpleNamedEntity.MaxNameLength, MinimumLength = SimpleNamedEntity.MinNameLength)]
    string Name,
    bool Active
);
