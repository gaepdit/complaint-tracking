using Cts.Domain.ActionTypes;
using System.ComponentModel.DataAnnotations;

namespace Cts.AppServices.ActionTypes;

public class ActionTypeViewDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;

    [UIHint("BoolActive")]
    public bool Active { get; init; }
}

public class ActionTypeCreateDto
{
    [Required(AllowEmptyStrings = false)]
    [StringLength(ActionType.MaxNameLength, MinimumLength = ActionType.MinNameLength)]
    public string Name { get; init; } = string.Empty;
}

public class ActionTypeUpdateDto
{
    public Guid Id { get; init; }

    [Required(AllowEmptyStrings = false)]
    [StringLength(ActionType.MaxNameLength, MinimumLength = ActionType.MinNameLength)]
    public string Name { get; init; } = string.Empty;

    public bool Active { get; init; }
}
