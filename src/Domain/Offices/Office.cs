using JetBrains.Annotations;
using MyAppRoot.Domain.Identity;

namespace MyAppRoot.Domain.Offices;

public class Office : AuditableEntity
{
    public const int MaxNameLength = 450;
    public const int MinNameLength = 2;

    [StringLength(MaxNameLength)]
    public string Name { get; private set; } = string.Empty;

    public bool Active { get; set; } = true;

    public List<ApplicationUser> StaffMembers { get; set; } = new();

    [UsedImplicitly] // Used by ORM.
    private Office() { }

    internal Office(Guid id, string name) : base(id) => SetName(name);

    internal void ChangeName(string name) => SetName(name);

    private void SetName(string name) =>
        Name = Guard.ValidLength(name.Trim(), MinNameLength, MaxNameLength);
}
