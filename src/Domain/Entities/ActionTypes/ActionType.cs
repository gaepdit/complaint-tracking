namespace Cts.Domain.Entities.ActionTypes;

public class ActionType : StandardNamedEntity
{
    public override int MinNameLength => AppConstants.MinimumNameLength;
    public override int MaxNameLength => AppConstants.MaximumNameLength;
    public ActionType() { }
    internal ActionType(Guid id, string name) : base(id, name) { }
}
