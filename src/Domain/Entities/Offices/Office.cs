using Cts.Domain.Entities.EntityBase;
using Cts.Domain.Identity;

namespace Cts.Domain.Entities.Offices;

public class Office : SimpleNamedEntity
{
    public Office(Guid id, string name) : base(id, name) { }

    public ApplicationUser? Assignor { get; set; }

    [UsedImplicitly]
    public ICollection<ApplicationUser> StaffMembers { get; set; } = new List<ApplicationUser>();
}
