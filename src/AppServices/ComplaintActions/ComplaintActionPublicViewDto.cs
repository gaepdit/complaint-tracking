using Cts.Domain.ActionTypes;

namespace Cts.AppServices.ComplaintActions;

public class ComplaintActionPublicViewDto
{
    public DateTime ActionDate { get; init; }
    public ActionType ActionType { get; init; } = null!;
    public string? Comments { get; set; }
}
