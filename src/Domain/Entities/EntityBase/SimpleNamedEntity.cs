using System.Text;

namespace Cts.Domain.Entities.EntityBase;

public abstract class SimpleNamedEntity : AuditableEntity
{
    // Constants

    public const int MaxNameLength = 450;
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

    // Display properties

    public string NameWithActivity
    {
        get
        {
            var sn = new StringBuilder();
            sn.Append(Name);
            if (!Active) sn.Append(" [Inactive]");
            return sn.ToString();
        }
    }
}
