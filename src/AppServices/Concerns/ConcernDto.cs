using Cts.Domain.BaseEntities;
using System.ComponentModel.DataAnnotations;

namespace Cts.AppServices.Concerns;

public class ConcernViewDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;

    [UIHint("BoolActive")]
    public bool Active { get; init; }
}

public class ConcernCreateDto
{
    [Required(AllowEmptyStrings = false)]
    [StringLength(SimpleNamedEntity.MaxNameLength, MinimumLength = SimpleNamedEntity.MinNameLength)]
    public string Name { get; init; } = string.Empty;
}

public class ConcernUpdateDto
{
    public Guid Id { get; init; }

    [Required(AllowEmptyStrings = false)]
    [StringLength(SimpleNamedEntity.MaxNameLength, MinimumLength = SimpleNamedEntity.MinNameLength)]
    public string Name { get; init; } = string.Empty;

    public bool Active { get; init; }
}
