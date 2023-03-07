namespace Cts.AppServices.ComplaintActions;

public class ComplaintActionPublicViewDto
{
    public DateTimeOffset ActionDate { get; init; }
    public string ActionTypeName { get; init; } = default!;
    public string? Comments { get; init; }
}
