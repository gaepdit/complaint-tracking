using Cts.Domain.Users;
using JetBrains.Annotations;

namespace Cts.Domain.Offices;

public class Office : AuditableEntity
{
    public const int MaxNameLength = 450;

    [StringLength(MaxNameLength)]
    public string Name { get; private set; } = string.Empty;

    public bool Active { get; set; } = true;

    public ApplicationUser? MasterUser { get; set; }

    public List<ApplicationUser> Users { get; set; } = new();

    [UsedImplicitly] // Used by ORM.
    private Office() { }

    internal Office(
        Guid id,
        string name,
        ApplicationUser? masterUser
    ) : base(id)
    {
        SetName(name);
        MasterUser = masterUser;
    }

    internal void ChangeName(string name) => SetName(name);

    private void SetName(string name) =>
        Name = Guard.ValidLength(name.Trim(), minLength: 1, maxLength: MaxNameLength);
}
