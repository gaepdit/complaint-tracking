using Cts.Domain.BaseEntities;
using System.ComponentModel.DataAnnotations;

namespace Cts.AppServices.BaseDto;

public abstract class SimpleNamedEntityViewDto : IDtoHasNameProperty
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;

    [UIHint("BoolActive")]
    public bool Active { get; init; }
}

public abstract class SimpleNamedEntityCreateDto
{
    [Required(AllowEmptyStrings = false)]
    [StringLength(SimpleNamedEntity.MaxNameLength, MinimumLength = SimpleNamedEntity.MinNameLength)]
    public string Name { get; init; } = string.Empty;
}

public abstract class SimpleNamedEntityUpdateDto
{
    public Guid Id { get; init; }

    [Required(AllowEmptyStrings = false)]
    [StringLength(SimpleNamedEntity.MaxNameLength, MinimumLength = SimpleNamedEntity.MinNameLength)]
    public string Name { get; init; } = string.Empty;

    public bool Active { get; init; }
}
