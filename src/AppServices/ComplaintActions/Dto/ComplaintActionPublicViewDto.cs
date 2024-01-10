namespace Cts.AppServices.ComplaintActions;

public record ComplaintActionPublicViewDto
{
    public DateOnly ActionDate { get; init; }
    public string ActionTypeName { get; init; } = default!;
    public string? Comments { get; init; }
}
