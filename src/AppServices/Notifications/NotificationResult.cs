namespace Cts.AppServices.Notifications;

public record NotificationResult
{
    private NotificationResult() => Success = true;
    private NotificationResult(string failureMessage) => FailureMessage = failureMessage;

    public bool Success { get; }
    public string FailureMessage { get; } = string.Empty;

    public static NotificationResult SuccessResult() => new();
    public static NotificationResult FailureResult(string failureMessage) => new(failureMessage);
}
