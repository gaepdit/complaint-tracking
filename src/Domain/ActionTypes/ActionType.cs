using JetBrains.Annotations;

namespace Cts.Domain.ActionTypes;

public class ActionType : AuditableEntity
{
    public const int MaxNameLength = 50;
    public const int MinNameLength = 2;

    [StringLength(MaxNameLength)]
    public string Name { get; private set; } = string.Empty;

    public bool Active { get; set; } = true;

    [UsedImplicitly] // Used by ORM.
    private ActionType() { }

    internal ActionType(Guid id, string name) : base(id) => SetName(name);

    internal void ChangeName(string name) => SetName(name);

    private void SetName(string name) =>
        Name = Guard.ValidLength(name.Trim(), minLength: MinNameLength, maxLength: MaxNameLength, nameof(name));
}
