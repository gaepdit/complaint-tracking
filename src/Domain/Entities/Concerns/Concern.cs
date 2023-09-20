namespace Cts.Domain.Entities.Concerns;

public class Concern : StandardNamedEntity
{
    public override int MinNameLength => AppConstants.MinimumNameLength;
    public override int MaxNameLength => AppConstants.MaximumNameLength;
    public Concern() { }
    internal Concern(Guid id, string name) : base(id, name) { }
}
