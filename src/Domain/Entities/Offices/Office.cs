using Cts.Domain.Identity;

namespace Cts.Domain.Entities.Offices;

public class Office : StandardNamedEntity
{
    public override int MinNameLength => AppConstants.MinimumNameLength;
    public override int MaxNameLength => AppConstants.MaximumNameLength;
    public static string[] IncludeAssignor => [nameof(Assignor)];

    public Office() { }
    internal Office(Guid id, string name) : base(id, name) { }

    public ApplicationUser? Assignor { get; set; }

    [UsedImplicitly]
    public ICollection<ApplicationUser> StaffMembers { get; set; } = new List<ApplicationUser>();
}
