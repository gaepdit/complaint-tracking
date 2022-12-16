using JetBrains.Annotations;

namespace Cts.Domain.BaseEntities;

public abstract class SimpleNamedEntity : AuditableEntity
{
    // Constants

    public const int MaxNameLength = 50;
    public const int MinNameLength = 2;

    // Constructors

    [UsedImplicitly] // Used by ORM.
    private SimpleNamedEntity() { }

    protected SimpleNamedEntity(Guid id, string name) : base(id) => SetName(name);

    // Properties

    [StringLength(MaxNameLength, MinimumLength = MinNameLength)]
    public string Name { get; private set; } = string.Empty;

    public bool Active { get; set; } = true;

    // Methods

    internal void ChangeName(string name) => SetName(name);

    private void SetName(string name) =>
        Name = Guard.ValidLength(name.Trim(), minLength: MinNameLength, maxLength: MaxNameLength);
}
