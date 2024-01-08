namespace Cts.AppServices.ComplaintActions;

public record ComplaintActionPublicViewDto
{
    public DateTimeOffset ActionDate { get; init; }
    public string ActionTypeName { get; init; } = default!;
    public string? Comments { get; init; }
}
