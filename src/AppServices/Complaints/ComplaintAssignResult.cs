using Cts.Domain.Entities.Complaints;

namespace Cts.AppServices.Complaints;

/// <summary>
/// The result of a <see cref="Complaint"/> assignment operation.
/// </summary>
public class ComplaintAssignResult : ComplaintCommandResult
{
    /// <summary>
    /// True if the <see cref="Complaint"/> was reassigned; otherwise, false.
    /// </summary>
    public bool IsReassigned { get; internal set; }
}
