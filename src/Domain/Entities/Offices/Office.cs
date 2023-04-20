using Cts.Domain.Identity;
using JetBrains.Annotations;

namespace Cts.Domain.Entities.Offices;

public class Office : AuditableEntity
{
    // Constants

    public const int MaxNameLength = 450;
    public const int MinNameLength = 2;

    // Constructors

    [UsedImplicitly] // Used by ORM.
    private Office() { }

    internal Office(Guid id, string name) : base(id) => SetName(name);

    // Properties

    [StringLength(MaxNameLength, MinimumLength = MinNameLength)]
    public string Name { get; private set; } = string.Empty;

    public bool Active { get; set; } = true;

    public ApplicationUser? Assignor { get; set; }

    public List<ApplicationUser> StaffMembers { get; set; } = new();

    // Methods

    internal void ChangeName(string name) => SetName(name);

    private void SetName(string name) =>
        Name = Guard.ValidLength(name.Trim(), MinNameLength, MaxNameLength);
}
